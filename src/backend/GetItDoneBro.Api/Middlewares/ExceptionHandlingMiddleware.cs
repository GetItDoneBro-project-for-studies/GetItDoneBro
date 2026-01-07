using GetItDoneBro.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GetItDoneBro.Api.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IWebHostEnvironment environment
)
{
    private const string ReferenceIdKey = "ReferenceId";
    private readonly bool isDevelopment = environment.IsDevelopment();

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BaseException baseException)
        {
            string referenceId = GenerateReferenceId();
            context.Items[ReferenceIdKey] = referenceId;

            var problemDetails = new ProblemDetails
            {
                Status = baseException.StatusCode,
                Type = baseException.ExceptionType,
                Title = baseException.Title,
                Detail = baseException.Detail
            };

            if (baseException.Errors.Any())
            {
                problemDetails.Extensions["errors"] = baseException.Errors;
            }

            problemDetails.Extensions["referenceId"] = referenceId;

            logger.LogWarning(
                baseException,
                "baseException caught: [{Type}] {Title} - {Detail} [RefId: {ReferenceId}]",
                baseException.ExceptionType,
                baseException.Title,
                baseException.Detail,
                referenceId
            );

            context.Response.StatusCode = baseException.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (UnauthorizedAccessException exception)
        {
            string referenceId = GenerateReferenceId();
            context.Items[ReferenceIdKey] = referenceId;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Type = "Unauthorized",
                Title = "Brak autoryzacji!",
                Detail = "Nie masz uprawnień do wykonania tej operacji."
            };

            if (isDevelopment)
            {
                problemDetails.Extensions["exception"] = exception.Message;
            }

            problemDetails.Extensions["referenceId"] = referenceId;

            logger.LogWarning(
                exception,
                "Unauthorized access attempt [RefId: {ReferenceId}]",
                referenceId
            );

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception exception)
        {
            string referenceId = GenerateReferenceId();
            context.Items[ReferenceIdKey] = referenceId;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "ServerError",
                Title = "Błąd serwera!",
                Detail = "Wystąpił nieoczekiwany błąd."
            };

            if (isDevelopment)
            {
                problemDetails.Extensions["exception"] = exception.Message;
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            problemDetails.Extensions["referenceId"] = referenceId;

            logger.LogError(
                exception,
                "Unexpected server error [RefId: {ReferenceId}]",
                referenceId
            );

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static string GenerateReferenceId()
    {
        return $"ERR-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..8]}";
    }
}
