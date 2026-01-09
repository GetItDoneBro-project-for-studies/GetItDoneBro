using System.Net;
using Flurl;
using Flurl.Http;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Domain.Models;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Infrastructure.Services;

public sealed class RequestProxyService(
    ITokenService tokenService,
    IHttpClientFactory httpClientFactory,
    ILogger<RequestProxyService> logger
) : IDisposable
{
    private readonly IHttpClientFactory httpClientFactory =
        httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

    private readonly ILogger<RequestProxyService> logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ITokenService tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    private bool disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<TResponse?> GetJsonAsync<TResponse>(Request request, CancellationToken cancellationToken)
    {
        var accessToken = await tokenService.GetAccessTokenAsync(cancellationToken);
        var httpClient = httpClientFactory.CreateClient("KeycloakProxyClient");
        using var flurlClient = new FlurlClient(httpClient);
        var url = new Url(request.Host).AppendPathSegment(request.Uri);

        try
        {
            logger.LogDebug(message: "Making GET request to {Url}", url);

            var response = await flurlClient
                .WithOAuthBearerToken(accessToken)
                .Request(url)
                .GetJsonAsync<TResponse>(cancellationToken: cancellationToken);
            return response;
        }
        catch (FlurlHttpException ex) when (ex.StatusCode == (int)HttpStatusCode.NotFound)
        {
            logger.LogWarning(exception: ex, message: "Resource not found at {Url}", url);
            return default;
        }
        catch (FlurlHttpException ex) when (ex.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            tokenService.InvalidateToken();
            throw;
        }
    }

    public async Task<ResponseObject<string>> PostAsync(Request request, CancellationToken cancellationToken)
    {
        var accessToken = await tokenService.GetAccessTokenAsync(cancellationToken);
        var httpClient = httpClientFactory.CreateClient("KeycloakProxyClient");
        using var flurlClient = new FlurlClient(httpClient);
        var url = new Url(request.Host).AppendPathSegment(request.Uri);

        try
        {
            logger.LogDebug(message: "Making POST request to {Url}", url);

            var flurlResponse = await flurlClient
                .WithOAuthBearerToken(accessToken)
                .Request(url)
                .PostJsonAsync(body: request.Data, cancellationToken: cancellationToken);

            var responseBody = await flurlResponse.ResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            return new ResponseObject<string>
            {
                Code = flurlResponse.StatusCode,
                Value = responseBody,
                Headers = flurlResponse.ResponseMessage.Headers
            };
        }
        catch (FlurlHttpException ex) when (ex.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            tokenService.InvalidateToken();
            throw;
        }
        catch (FlurlHttpException ex)
        {
            logger.LogError(
                exception: ex,
                message: "FlurlHttpException during POST to {Url}. Status: {StatusCode}",
                url,
                ex.StatusCode
            );

            var errorBody = await ex.GetResponseStringAsync();
            var responseHeaders = ex.Call?.Response?.ResponseMessage?.Headers;

            return new ResponseObject<string>
            {
                Code = ex.StatusCode ?? 500,
                Value = $"FlurlHttpException: {ex.Message}. Response: {errorBody}",
                Headers = responseHeaders
            };
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(exception: ex, message: "HttpRequestException during POST to {Url}", url);

            return new ResponseObject<string>
            {
                Code = 500,
                Value = $"HttpRequestException: {ex.Message}. Inner Exception: {ex.InnerException?.Message}",
                Headers = null
            };
        }
        catch (Exception ex)
        {
            logger.LogError(exception: ex, message: "Unhandled exception during POST to {Url}", url);

            return new ResponseObject<string>
            {
                Code = 500,
                Value = $"Unhandled Exception: {ex.Message}. StackTrace: {ex.StackTrace}",
                Headers = null
            };
        }
    }

    public async Task<ResponseObject<string>> PutAsync(Request request, CancellationToken cancellationToken)
    {
        var accessToken = await tokenService.GetAccessTokenAsync(cancellationToken);
        var httpClient = httpClientFactory.CreateClient("KeycloakProxyClient");
        using var flurlClient = new FlurlClient(httpClient);
        var url = new Url(request.Host).AppendPathSegment(request.Uri);

        try
        {
            logger.LogDebug(message: "Making PUT request to {Url}", url);

            var flurlResponse = await flurlClient
                .WithOAuthBearerToken(accessToken)
                .Request(url)
                .PutJsonAsync(body: request.Data, cancellationToken: cancellationToken);

            var responseBody = await flurlResponse.ResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            return new ResponseObject<string>
            {
                Code = flurlResponse.StatusCode,
                Value = responseBody,
                Headers = flurlResponse.ResponseMessage.Headers
            };
        }
        catch (FlurlHttpException ex) when (ex.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            tokenService.InvalidateToken();
            throw;
        }
        catch (FlurlHttpException ex)
        {
            logger.LogError(
                exception: ex,
                message: "FlurlHttpException during PUT to {Url}. Status: {StatusCode}",
                url,
                ex.StatusCode
            );

            var errorBody = await ex.GetResponseStringAsync();
            var responseHeaders = ex.Call?.Response?.ResponseMessage?.Headers;

            return new ResponseObject<string>
            {
                Code = ex.StatusCode ?? 500,
                Value = $"FlurlHttpException: {ex.Message}. Response: {errorBody}",
                Headers = responseHeaders
            };
        }
        catch (Exception ex)
        {
            logger.LogError(exception: ex, message: "Exception during PUT to {Url}", url);

            return new ResponseObject<string>
            {
                Code = 500,
                Value = $"Generic Exception: {ex.Message}",
                Headers = null
            };
        }
    }

    public async Task<ResponseObject<string>> DeleteAsync(Request request, CancellationToken cancellationToken)
    {
        var accessToken = await tokenService.GetAccessTokenAsync(cancellationToken);
        var httpClient = httpClientFactory.CreateClient("KeycloakProxyClient");
        using var flurlClient = new FlurlClient(httpClient);
        var url = new Url(request.Host).AppendPathSegment(request.Uri);

        try
        {
            logger.LogDebug(message: "Making DELETE request to {Url}", url);

            var flurlRequest = flurlClient.WithOAuthBearerToken(accessToken).Request(url);

            IFlurlResponse flurlResponse = request.Data != null
                ? await flurlRequest
                    .SendJsonAsync(verb: HttpMethod.Delete, body: request.Data, cancellationToken: cancellationToken)
                : await flurlRequest
                    .DeleteAsync(cancellationToken: cancellationToken);

            var responseBody = await flurlResponse.ResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            return new ResponseObject<string>
            {
                Code = flurlResponse.StatusCode,
                Value = responseBody,
                Headers = flurlResponse.ResponseMessage.Headers
            };
        }
        catch (FlurlHttpException ex) when (ex.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            tokenService.InvalidateToken();
            throw;
        }
        catch (FlurlHttpException ex)
        {
            logger.LogError(
                exception: ex,
                message: "FlurlHttpException during DELETE to {Url}. Status: {StatusCode}",
                url,
                ex.StatusCode
            );

            var errorBody = await ex.GetResponseStringAsync();
            var responseHeaders = ex.Call?.Response?.ResponseMessage?.Headers;

            return new ResponseObject<string>
            {
                Code = ex.StatusCode ?? 500,
                Value = $"FlurlHttpException: {ex.Message}. Response: {errorBody}",
                Headers = responseHeaders
            };
        }
        catch (Exception ex)
        {
            logger.LogError(exception: ex, message: "Exception during DELETE to {Url}", url);

            return new ResponseObject<string>
            {
                Code = 500,
                Value = $"Generic Exception: {ex.Message}",
                Headers = null
            };
        }
    }

    private void Dispose(bool disposing)
    {
        if (!disposed && disposing)
        {
            disposed = true;
        }
    }
}
