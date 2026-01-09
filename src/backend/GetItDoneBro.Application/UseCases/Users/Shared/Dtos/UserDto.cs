using GetItDoneBro.Domain.Proxies.KeyCloak;

namespace GetItDoneBro.Application.UseCases.Users.Shared.Dtos;

public record UserDto(
    Guid Id,
    string Username,
    string? FirstName,
    string? LastName,
    string? Email,
    bool EmailVerified,
    bool Enabled,
    IEnumerable<RealmRole> RealmRoles
)
{
    public UserDto(User user) : this(
        user.Id,
        user.Username,
        user.FirstName,
        user.LastName,
        user.Email,
        user.EmailVerified,
        user.Enabled,
        user.RealmRoles
    )
    { }
}
