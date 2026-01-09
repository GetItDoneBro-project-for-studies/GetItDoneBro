using System.Security.Claims;
using GetItDoneBro.Domain.Exceptions;
using GetItDoneBro.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GetItDoneBro.Infrastructure.Services;

public class UserResolverService(IHttpContextAccessor httpContextAccessor) : IUserResolver
{
    private bool? isEmailVerified;
    private Guid? userId;

    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool IsEmailVerified
    {
        get
        {
            ValidateClaimsPrincipal(User);

            if (isEmailVerified.HasValue)
            {
                return isEmailVerified.Value;
            }

            var emailVerifiedClaim = User?.FindFirst("email_verified")?.Value;
            if (string.IsNullOrEmpty(emailVerifiedClaim) || !bool.TryParse(emailVerifiedClaim, result: out var isVerified))
            {
                throw new InvalidOperationException("Nie znaleziono roszczenia 'email_verified' lub jest puste.");
            }

            isEmailVerified = isVerified;
            return isEmailVerified.Value;
        }
    }

    public Guid UserId
    {
        get
        {
            ValidateClaimsPrincipal(User);

            if (userId.HasValue)
            {
                return userId.Value;
            }

            if (!Guid.TryParse(input: User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0", result: out var id))
            {
                throw new InvalidOperationException($"Nie znaleziono roszczenia '{nameof(UserId)}' lub nie jest liczbą.");
            }

            userId = id;

            return userId.Value;
        }
    }

    public string FullName
    {
        get
        {
            ValidateClaimsPrincipal(User);

            if (!string.IsNullOrEmpty(field))
            {
                return field;
            }

            field = User?.FindFirst(ClaimTypes.Name)?.Value ?? User?.FindFirst("name")?.Value ?? User?.FindFirst("preferred_username")?.Value;

            if (!string.IsNullOrEmpty(field))
            {
                return field;
            }

            var givenName = User?.FindFirst("given_name")?.Value;
            var familyName = User?.FindFirst("family_name")?.Value;

            field = !string.IsNullOrEmpty(givenName) && !string.IsNullOrEmpty(familyName)
                ? $"{givenName} {familyName}"
                : throw new InvalidOperationException(
                    $"Nie znaleziono roszczenia '{nameof(FullName)}' lub jest puste.");

            return field;
        }

        private set;
    }
    

    private static void ValidateClaimsPrincipal(ClaimsPrincipal? principal)
    {
        if (principal is null)
        {
            throw new IntegrityException("HttpContext lub ClaimsPrincipal jest niedostępny.");
        }
    }
}
