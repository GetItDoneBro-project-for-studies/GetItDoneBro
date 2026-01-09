using System.Text.Json;
using System.Text.Json.Serialization;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Domain.Exceptions;
using GetItDoneBro.Domain.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GetItDoneBro.Infrastructure.Services;

public sealed class TokenService(
    IMemoryCache cache,
    IOptions<KeycloakOptions> options,
    HttpClient httpClient,
    ILogger<TokenService> logger
    )
    : ITokenService, IDisposable
{
    private const string TokenCacheKey = "keycloak_access_token";
    private const string OpenIdConfigCacheKey = "keycloak_openid_config";
    private static readonly TimeSpan OpenIdConfigCacheExpiration = TimeSpan.FromHours(24);
    private readonly IMemoryCache cache = cache ?? throw new ArgumentNullException(nameof(cache));
    private readonly HttpClient httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly ILogger<TokenService> logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IOptions<KeycloakOptions> options = options ?? throw new ArgumentNullException(nameof(options));
    private readonly SemaphoreSlim semaphore = new(initialCount: 1, maxCount: 1);
    private bool disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(disposed, nameof(TokenService));

        if (cache.TryGetValue(key: TokenCacheKey, value: out string? cachedToken) && !string.IsNullOrEmpty(cachedToken))
        {
            return cachedToken;
        }

        await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            ObjectDisposedException.ThrowIf(disposed, nameof(TokenService));

            if (cache.TryGetValue(key: TokenCacheKey, value: out cachedToken) && !string.IsNullOrEmpty(cachedToken))
            {
                return cachedToken;
            }

            var tokenModel = await FetchTokenAsync(cancellationToken).ConfigureAwait(false);

            var cacheExpiration = TimeSpan.FromSeconds(tokenModel.ExpiresIn - 60);
            if (cacheExpiration > TimeSpan.Zero)
            {
                cache.Set(key: TokenCacheKey, value: tokenModel.AccessToken, absoluteExpirationRelativeToNow: cacheExpiration);
            }

            return tokenModel.AccessToken;
        }
        finally
        {
            semaphore.Release();
        }
    }

    public void InvalidateToken()
    {
        cache.Remove(TokenCacheKey);
        logger.LogDebug("Access token invalidated and removed from cache");
    }

    private async Task<string> GetTokenEndpointAsync(CancellationToken cancellationToken)
    {
        if (cache.TryGetValue(key: OpenIdConfigCacheKey, value: out string? cachedEndpoint) && !string.IsNullOrEmpty(cachedEndpoint))
        {
            return cachedEndpoint;
        }

        var wellKnownUrl = $"{options.Value.AuthServerUrl}/realms/{options.Value.Realm}/.well-known/openid-configuration";

        try
        {
            logger.LogDebug(message: "Fetching OpenID configuration from {Url}", wellKnownUrl);

            var response = await httpClient.GetStringAsync(requestUri: wellKnownUrl, cancellationToken: cancellationToken).ConfigureAwait(false);
            using var document = JsonDocument.Parse(response);

            if (document.RootElement.TryGetProperty(propertyName: "token_endpoint", value: out var tokenEndpointElement))
            {
                var tokenEndpoint = tokenEndpointElement.GetString();
                if (!string.IsNullOrEmpty(tokenEndpoint))
                {
                    cache.Set(key: OpenIdConfigCacheKey, value: tokenEndpoint, absoluteExpirationRelativeToNow: OpenIdConfigCacheExpiration);
                    logger.LogDebug(message: "Cached token_endpoint: {TokenEndpoint}", tokenEndpoint);
                    return tokenEndpoint;
                }
            }

            throw new ConfigurationException("TokenEndpoint", "Nie znaleziono token_endpoint w konfiguracji OpenID");
        }
        catch (Exception ex) when (ex is not ConfigurationException)
        {
            logger.LogError(exception: ex, message: "Failed to fetch OpenID configuration from {Url}", wellKnownUrl);
            throw new ConfigurationException($"Nie udało się pobrać konfiguracji OpenID z {wellKnownUrl}", ex);
        }
    }

    private async Task<AccessTokenModel> FetchTokenAsync(CancellationToken cancellationToken)
    {
        var tokenEndpoint = await GetTokenEndpointAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            logger.LogDebug(message: "Fetching OAuth token from {TokenEndpoint}", tokenEndpoint);

            using var formData = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>(key: "client_id", value: options.Value.Resource), new KeyValuePair<string, string>(key: "client_secret", value: options.Value.Credentials.Secret), new KeyValuePair<string, string>(key: "grant_type", value: "client_credentials")
                }
            );

            using var response = await httpClient.PostAsync(requestUri: tokenEndpoint, content: formData, cancellationToken: cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                logger.LogError(
                    message: "Failed to obtain OAuth token. Status: {StatusCode}, Content: {Content}",
                    response.StatusCode,
                    errorContent
                );
                throw new IntegrityException($"Nie udało się pobrać tokenu OAuth. Status: {response.StatusCode}");
            }

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var accessTokenModel = JsonSerializer.Deserialize<AccessTokenModel>(responseBody);

            if (accessTokenModel == null)
            {
                logger.LogError("Failed to deserialize access token response");
                throw new IntegrityException("Nie udało się zdeserializować tokenu dostępu");
            }

            logger.LogDebug(message: "Successfully obtained OAuth token, expires in {ExpiresIn} seconds", accessTokenModel.ExpiresIn);
            return accessTokenModel;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            logger.LogError(
                exception: ex,
                message: "Timeout during OAuth token request to {TokenEndpoint}. Request was canceled due to timeout.",
                tokenEndpoint
            );
            throw new IntegrityException($"Timeout podczas pobierania tokenu OAuth z {tokenEndpoint}");
        }
        catch (TaskCanceledException ex)
        {
            logger.LogError(
                exception: ex,
                message: "OAuth token request to {TokenEndpoint} was canceled. CancellationToken.IsCancellationRequested: {IsCanceled}",
                tokenEndpoint,
                cancellationToken.IsCancellationRequested
            );
            throw new IntegrityException($"Żądanie tokenu OAuth zostało anulowane dla {tokenEndpoint}");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(exception: ex, message: "HTTP error fetching OAuth token from {TokenEndpoint}", tokenEndpoint);
            throw new IntegrityException($"Błąd HTTP podczas pobierania tokenu OAuth z {tokenEndpoint}");
        }
    }

    private void Dispose(bool disposing)
    {
        if (disposed || !disposing)
        {
            return;
        }

        semaphore.Dispose();
        disposed = true;
    }

    private sealed record AccessTokenModel(
        [property: JsonPropertyName("access_token")]
        string AccessToken,
        [property: JsonPropertyName("token_type")]
        string TokenType,
        [property: JsonPropertyName("expires_in")]
        int ExpiresIn
        );
}
