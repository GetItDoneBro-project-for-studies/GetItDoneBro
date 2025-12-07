using System.Security.Claims;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace GetItDoneBro.Infrastructure.Services;

public class UserResolverService(IHttpContextAccessor httpContextAccessor) : IUserResolver
{
    private string? fullName;
    private bool? isEmailVerified;
    private Guid? userId;

    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool GetIsEmailVerified()
    {
        ValidateClaimsPrincipal(User);

        if (isEmailVerified.HasValue)
        {
            return isEmailVerified.Value;
        }

        var emailVerifiedClaim = User?.FindFirst("email_verified")?.Value;
        if (string.IsNullOrEmpty(emailVerifiedClaim) ||
            !bool.TryParse(emailVerifiedClaim, result: out var isVerified))
        {
            throw new ValidationException("email_verified",
                "Nie znaleziono roszczenia 'email_verified' lub jest puste.");
        }

        isEmailVerified = isVerified;
        return isEmailVerified.Value;
    }

    public bool TryGetIsEmailVerified(out bool isEmailVerified)
    {
        try
        {
            isEmailVerified = GetIsEmailVerified();
            return true;
        }
        catch
        {
            isEmailVerified = default;
            return false;
        }
    }

    public Guid GetUserId()
    {
        ValidateClaimsPrincipal(User);

        if (userId.HasValue)
        {
            return userId.Value;
        }

        if (!Guid.TryParse(input: User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0", result: out var id))
        {
            throw new ValidationException("sub", "Nie znaleziono roszczenia 'sub' lub nie jest liczbą.");
        }

        userId = id;

        return userId.Value;
    }

    public bool TryGetUserId(out Guid userId)
    {
        try
        {
            userId = GetUserId();
            return true;
        }
        catch
        {
            userId = Guid.Empty;
            return false;
        }
    }

    public string GetFullName()
    {
        ValidateClaimsPrincipal(User);

        if (!string.IsNullOrEmpty(fullName))
        {
            return fullName;
        }

        fullName = User?.FindFirst(ClaimTypes.Name)?.Value ??
                   User?.FindFirst("name")?.Value ?? User?.FindFirst("preferred_username")?.Value;

        if (!string.IsNullOrEmpty(fullName))
        {
            return fullName;
        }

        var givenName = User?.FindFirst("given_name")?.Value;
        var familyName = User?.FindFirst("family_name")?.Value;

        fullName = !string.IsNullOrEmpty(givenName) && !string.IsNullOrEmpty(familyName)
            ? $"{givenName} {familyName}"
            : throw new ValidationException("name", "Nie znaleziono roszczenia 'name' lub jest puste.");

        return fullName;
    }

    public bool TryGetFullName(out string fullName)
    {
        try
        {
            fullName = GetFullName();
            return true;
        }
        catch
        {
            fullName = string.Empty;
            return false;
        }
    }

    public void ClearCache()
    {
        fullName = null;
        isEmailVerified = null;
        userId = null;
    }

    private static void ValidateClaimsPrincipal(ClaimsPrincipal? principal)
    {
        if (principal is null)
        {
            throw new IntegrityException("HttpContext lub ClaimsPrincipal jest niedostępny.");
        }
    }
}
