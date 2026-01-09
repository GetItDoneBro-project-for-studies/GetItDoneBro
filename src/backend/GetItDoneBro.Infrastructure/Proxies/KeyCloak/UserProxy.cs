using System.Net;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Domain.Enums;
using GetItDoneBro.Domain.Exceptions;
using GetItDoneBro.Domain.Models;
using GetItDoneBro.Domain.Options;
using GetItDoneBro.Domain.Proxies.KeyCloak;
using GetItDoneBro.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GetItDoneBro.Infrastructure.Proxies.KeyCloak;

internal sealed class UserProxy(
    IOptions<KeycloakOptions> keycloakOptions,
    RequestProxyService requestProxyService,
    ICacheService cacheService,
    ILogger<UserProxy> logger) : IUserProxy
{
    private const string UsersCacheKey = "keycloak_users_all";
    private static readonly TimeSpan UsersCacheExpiration = TimeSpan.FromMinutes(5);

    public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var cachedUsers = await cacheService.GetAsync<List<User>>(key: UsersCacheKey, cancellationToken: cancellationToken);
        if (cachedUsers != null)
        {
            return cachedUsers;
        }

        var request = new Request(host: keycloakOptions.Value.AuthServerUrl, uri: new Uri(uriString: $"/admin/realms/{keycloakOptions.Value.Realm}/users", uriKind: UriKind.Relative));
        var users = await requestProxyService.GetJsonAsync<IEnumerable<User>>(request: request, cancellationToken: cancellationToken) ?? [];

        var usersList = users.ToList();
        await cacheService.SetAsync(key: UsersCacheKey, value: usersList, expiration: UsersCacheExpiration, cancellationToken: cancellationToken);

        return usersList;
    }

    public async Task<User> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new Request(host: keycloakOptions.Value.AuthServerUrl, uri: new Uri(uriString: $"/admin/realms/{keycloakOptions.Value.Realm}/users/{id}", uriKind: UriKind.Relative));
        try
        {
            var user = await requestProxyService.GetJsonAsync<User>(request: request, cancellationToken: cancellationToken);
            if (user == null)
            {
                logger.LogDebug(message: "Użytkownik o ID {UserId} nie został znaleziony w KeyCloak", id);
                throw new NotFoundException("User", $"Użytkownik o ID {id} nie został znaleziony w KeyCloak");
            }
            return user;
        }
        catch (Exception ex) when (ex is not NotFoundException)
        {
            throw;
        }
    }

    public async Task<User?> TryGetUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new Request(host: keycloakOptions.Value.AuthServerUrl, uri: new Uri(uriString: $"/admin/realms/{keycloakOptions.Value.Realm}/users/{id}", uriKind: UriKind.Relative));
        const int maxRetries = 3;
        const int delayMs = 200;

        for (var i = 0; i < maxRetries; i++)
        {
            try
            {
                var user = await requestProxyService.GetJsonAsync<User>(request: request, cancellationToken: cancellationToken);

                if (user != null && user.Id == id && !string.IsNullOrEmpty(user.Username) && !string.IsNullOrEmpty(user.Email))
                {
                    return user;
                }

                if (i < maxRetries - 1)
                {
                    await Task.Delay(millisecondsDelay: delayMs, cancellationToken: cancellationToken);
                } else
                {
                    logger.LogDebug(
                        message: "Użytkownik o ID {UserId} został pobrany, ale nie ma wszystkich wymaganych właściwości po {MaxRetries} próbach. Username: {Username}, Email: {Email}",
                        id,
                        maxRetries,
                        user?.Username,
                        user?.Email
                    );
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogDebug(
                    exception: ex,
                    message: "Nie udało się pobrać użytkownika o ID {UserId}. Próba {Attempt}/{MaxRetries}. Message: {Message}, RequestUrl: {RequestUrl}",
                    id,
                    i + 1,
                    maxRetries,
                    ex.Message,
                    $"{request.Host}{request.Uri}"
                );

                if (i >= maxRetries - 1)
                {
                    return null;
                }
                await Task.Delay(millisecondsDelay: delayMs, cancellationToken: cancellationToken);
            }
        }
        return null;
    }

    public async Task<Guid> AddUserAsync(KeyCloakUser user, CancellationToken cancellationToken = default)
    {
        var request = new Request(host: keycloakOptions.Value.AuthServerUrl, uri: new Uri(uriString: $"/admin/realms/{keycloakOptions.Value.Realm}/users/", uriKind: UriKind.Relative), data: user);
        var response = await requestProxyService.PostAsync(request: request, cancellationToken: cancellationToken);

        switch (response.Code)
        {
            case (int)HttpStatusCode.Created:
                if (response.Headers?.Location?.Segments is not null && response.Headers.Location.Segments.Length > 0)
                {
                    var lastSegment = response.Headers.Location.Segments.LastOrDefault()?.TrimEnd('/');
                    if (Guid.TryParse(input: lastSegment, result: out var userId))
                    {
                        await cacheService.RemoveAsync(key: UsersCacheKey, cancellationToken: cancellationToken);
                        if (!keycloakOptions.Value.SkipEmailSending)
                        {
                            await SendExecuteActionsEmailAsync(userId: userId, actions: [KeyCloakUserAction.VERIFY_EMAIL], cancellationToken: cancellationToken);
                        }
                        return userId;
                    }
                }
                logger.LogDebug(
                    message: "Użytkownik został utworzony, ale nie udało się pobrać jego identyfikatora. Request: {RequestUrl}, Payload: {@Payload}, ResponseHeaders: {@Headers}",
                    $"{keycloakOptions.Value.AuthServerUrl}/admin/realms/{keycloakOptions.Value.Realm}/users/",
                    user,
                    response.Headers
                );
                logger.LogError("Użytkownik został utworzony, ale nie udało się pobrać jego identyfikatora.");
                throw new UserException(propertyName: "UserId", errorMessage: "Użytkownik został utworzony, ale nie udało się pobrać jego identyfikatora.");

            case (int)HttpStatusCode.Conflict:
                logger.LogDebug(
                    message: "Użytkownik o podanej nazwie lub emailu już istnieje. Request: {RequestUrl}, Payload: {@Payload}, Response: {ResponseValue}",
                    $"{keycloakOptions.Value.AuthServerUrl}/admin/realms/{keycloakOptions.Value.Realm}/users/",
                    user,
                    response.Value
                );
                logger.LogError("Użytkownik o podanej nazwie lub emailu już istnieje.");
                throw new ValidationException(propertyName: "Użytkownik", errorMessage: "Użytkownik o podanej nazwie lub emailu już istnieje.");
            case (int)HttpStatusCode.BadRequest:
                logger.LogDebug(
                    message: "Podane dane użytkownika są nieprawidłowe. Request: {RequestUrl}, Payload: {@Payload}, Response: {ResponseValue}",
                    $"{keycloakOptions.Value.AuthServerUrl}/admin/realms/{keycloakOptions.Value.Realm}/users/",
                    user,
                    response.Value
                );
                logger.LogError("Podane dane użytkownika są nieprawidłowe.");
                throw new ValidationException(propertyName: "Użytkownik", errorMessage: "Podane dane użytkownika są nieprawidłowe.");
            default:
                logger.LogError(
                    message: "Wystąpił błąd podczas tworzenia użytkownika. Kod: {StatusCode}, Request: {RequestUrl}, Payload: {@Payload}, Response: {ResponseValue}",
                    response.Code,
                    $"{keycloakOptions.Value.AuthServerUrl}/admin/realms/{keycloakOptions.Value.Realm}/users/",
                    user,
                    response.Value
                );
                logger.LogError(message: "Wystąpił błąd podczas tworzenia użytkownika. Kod: {StatusCode}", response.Code);
                throw new UserException(propertyName: "Użytkownik", errorMessage: "Wystąpił błąd podczas tworzenia użytkownika.");
        }
    }

    public async Task SendExecuteActionsEmailAsync(Guid userId, IEnumerable<KeyCloakUserAction> actions, CancellationToken cancellationToken = default)
    {
        try
        {
            var actionStrings = actions.Select(a => a.ToString());

            var request = new Request(
                host: keycloakOptions.Value.AuthServerUrl,
                uri: new Uri(uriString: $"/admin/realms/{keycloakOptions.Value.Realm}/users/{userId}/execute-actions-email", uriKind: UriKind.Relative),
                data: actionStrings
            );

            var response = await requestProxyService.PutAsync(request: request, cancellationToken: cancellationToken);

            if (response.Code != (int)HttpStatusCode.OK && response.Code != (int)HttpStatusCode.NoContent)
            {
                logger.LogDebug(
                    message: "Nie udało się wysłać emaila z akcjami do użytkownika o ID {UserId}. Kod odpowiedzi: {StatusCode}, Request: {RequestUrl}, Payload: {@Payload}, Response: {ResponseValue}",
                    userId,
                    response.Code,
                    $"{keycloakOptions.Value.AuthServerUrl}/admin/realms/{keycloakOptions.Value.Realm}/users/{userId}/execute-actions-email",
                    actionStrings,
                    response.Value
                );

                logger.LogError(message: "Nie udało się wysłać emaila z akcjami do użytkownika o ID {UserId}. Kod odpowiedzi: {StatusCode}", userId, response.Code);
                throw new UserException(propertyName: "Email", errorMessage: "Nie udało się wysłać emaila z akcjami do użytkownika.");
            }
        }
        catch (Exception ex) when (ex is not UserException)
        {
            logger.LogDebug(
                exception: ex,
                message: "Wystąpił błąd podczas wysyłania emaila z akcjami do użytkownika o ID {UserId}. Message: {Message}, StackTrace: {StackTrace}, RequestUrl: {RequestUrl}",
                userId,
                ex.Message,
                ex.StackTrace,
                $"{keycloakOptions.Value.AuthServerUrl}/admin/realms/{keycloakOptions.Value.Realm}/users/{userId}/execute-actions-email"
            );

            logger.LogError(exception: ex, message: "Błąd krytyczny podczas wysyłania emaila z akcjami do użytkownika o ID {UserId}.", userId);
            throw new UserException(propertyName: "Email", errorMessage: "Wystąpił błąd podczas wysyłania emaila z akcjami do użytkownika.");
        }
    }

    public async Task DisableUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await GetUserAsync(id: userId, cancellationToken: cancellationToken);

            var updatedUser = KeyCloakUser.Create(
                username: user.Username,
                email: user.Email!,
                firstName: user.FirstName,
                lastName: user.LastName,
                enabled: false
            );

            var request = new Request(
                host: keycloakOptions.Value.AuthServerUrl,
                uri: new Uri(uriString: $"/admin/realms/{keycloakOptions.Value.Realm}/users/{userId}", uriKind: UriKind.Relative),
                data: updatedUser
            );

            var response = await requestProxyService.PutAsync(request: request, cancellationToken: cancellationToken);

            if (response.Code != (int)HttpStatusCode.OK && response.Code != (int)HttpStatusCode.NoContent)
            {
                logger.LogDebug(
                    message: "Nie udało się wyłączyć konta użytkownika o ID {UserId}. Kod odpowiedzi: {StatusCode}, Request: {RequestUrl}, Response: {ResponseValue}",
                    userId,
                    response.Code,
                    $"{keycloakOptions.Value.AuthServerUrl}/admin/realms/{keycloakOptions.Value.Realm}/users/{userId}",
                    response.Value
                );

                logger.LogError(message: "Nie udało się wyłączyć konta użytkownika o ID {UserId}. Kod odpowiedzi: {StatusCode}", userId, response.Code);
                throw new UserException(propertyName: "Konto", errorMessage: "Nie udało się wyłączyć konta użytkownika.");
            }

            await cacheService.RemoveAsync(key: UsersCacheKey, cancellationToken: cancellationToken);
        }
        catch (Exception ex) when (ex is not UserException)
        {
            logger.LogDebug(
                exception: ex,
                message: "Nie udało się wyłączyć użytkownika o ID {UserId}. Message: {Message}, StackTrace: {StackTrace}, RequestUrl: {RequestUrl}",
                userId,
                ex.Message,
                ex.StackTrace,
                $"{keycloakOptions.Value.AuthServerUrl}/admin/realms/{keycloakOptions.Value.Realm}/users/{userId}"
            );
            logger.LogError(exception: ex, message: "Błąd krytyczny podczas wyłączania użytkownika o ID {UserId}.", userId);
            throw new UserException(propertyName: "Użytkownik", errorMessage: "Wystąpił błąd podczas wyłączania użytkownika.");
        }
    }

    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new Request(
                host: keycloakOptions.Value.AuthServerUrl,
                uri: new Uri(uriString: $"/admin/realms/{keycloakOptions.Value.Realm}/users/{userId}", uriKind: UriKind.Relative)
            );

            var response = await requestProxyService.DeleteAsync(request: request, cancellationToken: cancellationToken);

            if (response.Code != (int)HttpStatusCode.OK && response.Code != (int)HttpStatusCode.NoContent)
            {
                logger.LogDebug(
                    message: "Nie udało się usunąć użytkownika o ID {UserId}. Kod odpowiedzi: {StatusCode}, Request: {RequestUrl}, Response: {ResponseValue}",
                    userId,
                    response.Code,
                    $"{keycloakOptions.Value.AuthServerUrl}/admin/realms/{keycloakOptions.Value.Realm}/users/{userId}",
                    response.Value
                );
                throw new UserException(propertyName: "Użytkownik", errorMessage: "Wystąpił błąd podczas usuwania użytkownika.");
            }

            await cacheService.RemoveAsync(key: UsersCacheKey, cancellationToken: cancellationToken);
        }
        catch (Exception ex) when (ex is not UserException)
        {
            logger.LogDebug(
                exception: ex,
                message: "Nie udało się usunąć użytkownika o ID {UserId}. Message: {Message}, StackTrace: {StackTrace}, RequestUrl: {RequestUrl}",
                userId,
                ex.Message,
                ex.StackTrace,
                $"{keycloakOptions.Value.AuthServerUrl}/admin/realms/{keycloakOptions.Value.Realm}/users/{userId}"
            );
            logger.LogError(exception: ex, message: "Błąd krytyczny podczas usuwania użytkownika o ID {UserId}.", userId);
            throw new UserException(propertyName: "Użytkownik", errorMessage: "Wystąpił błąd podczas usuwania użytkownika.");
        }
    }
}
