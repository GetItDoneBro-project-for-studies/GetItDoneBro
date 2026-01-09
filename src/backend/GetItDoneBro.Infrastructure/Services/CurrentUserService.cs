using System.Security.Claims;
using GetItDoneBro.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace GetItDoneBro.Infrastructure.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? KeycloakId => httpContextAccessor.HttpContext?.User
        .FindFirstValue(ClaimTypes.NameIdentifier)
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue("sub");

    public string? Email => httpContextAccessor.HttpContext?.User
        .FindFirstValue(ClaimTypes.Email)
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue("email");

    public string? DisplayName => httpContextAccessor.HttpContext?.User
        .FindFirstValue("name")
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue("preferred_username")
        ?? Email;

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}