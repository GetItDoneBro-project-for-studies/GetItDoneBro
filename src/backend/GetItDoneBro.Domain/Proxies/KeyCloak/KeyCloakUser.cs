namespace GetItDoneBro.Domain.Proxies.KeyCloak;

public class KeyCloakUser
{

    private KeyCloakUser()
    {
    }
    public string username { get; init; } = string.Empty;
    public bool enabled { get; init; }
    public string email { get; init; } = string.Empty;
    public bool emailVerified { get; init; }
    public string? firstName { get; init; }
    public string? lastName { get; init; }

    public static KeyCloakUser Create(
        string username,
        string email,
        string? firstName,
        string? lastName,
        bool enabled = true,
        bool emailVerified = true
        )
    {
        return new KeyCloakUser
        {
            username = StringUtils.NormalizeInput(username) ?? string.Empty,
            enabled = enabled,
            email = StringUtils.NormalizeInput(email) ?? string.Empty,
            emailVerified = emailVerified,
            firstName = StringUtils.NormalizeInput(firstName),
            lastName = StringUtils.NormalizeInput(lastName)
        };
    }
}
