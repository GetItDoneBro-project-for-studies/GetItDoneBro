using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Domain.Options;

public sealed class KeycloakOptions : IAppOptions
{
    public static string ConfigSectionPath => "Keycloak";
    public string? Authority { get; set; }
    public string Realm { get; set; }
    public string Audience { get; set; }
    public bool RequireHttpsMetadata { get; set; } = true;

    public bool IsValid(bool isDevelopment)
    {
        if (isDevelopment)
        {
            return !string.IsNullOrWhiteSpace(Realm) && !string.IsNullOrWhiteSpace(Audience);
        }

        return !string.IsNullOrWhiteSpace(Authority)
               && !string.IsNullOrWhiteSpace(Realm)
               && !string.IsNullOrWhiteSpace(Audience);
    }
}
