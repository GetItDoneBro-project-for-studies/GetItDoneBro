namespace GetItDoneBro.Application.Common.Interfaces.Services;

public interface ICurrentUserService
{
    string? KeycloakId { get; }
    string? Email { get; }
    string? DisplayName { get; }
    bool IsAuthenticated { get; }
}