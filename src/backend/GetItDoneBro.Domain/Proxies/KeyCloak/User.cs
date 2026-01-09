namespace GetItDoneBro.Domain.Proxies.KeyCloak;

public record User(
    Guid Id,
    string Username,
    string? FirstName,
    string? LastName,
    string? Email,
    bool EmailVerified,
    bool Enabled,
    IEnumerable<RealmRole> RealmRoles
    );
