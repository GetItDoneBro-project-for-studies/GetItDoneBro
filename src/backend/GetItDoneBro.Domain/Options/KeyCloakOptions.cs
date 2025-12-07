using System.ComponentModel.DataAnnotations;
using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Domain.Options;

public class KeycloakOptions : IAppOptions, IValidatableObject
{
    public static string ConfigSectionPath => "KeycloakOptions";

    public string Host { get; set; } = string.Empty;

    public string Realm { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string Audience { get; set; } = "account";

    public bool RequireHttpsMetadata { get; set; }

    public bool SkipEmailSending { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        
        if (string.IsNullOrWhiteSpace(Host))
        {
            yield return new ValidationResult(
                "Keycloak Host is required",
                [nameof(Host)]
            );
        }
        else if (!Uri.IsWellFormedUriString(Host, UriKind.Absolute))
        {
            yield return new ValidationResult(
                "Keycloak Host must be a valid absolute URI",
                [nameof(Host)]
            );
        }

        if (string.IsNullOrWhiteSpace(Realm))
        {
            yield return new ValidationResult(
                "Keycloak Realm is required",
                [nameof(Realm)]
            );
        }

        if (string.IsNullOrWhiteSpace(ClientId))
        {
            yield return new ValidationResult(
                "Keycloak ClientId is required",
                [nameof(ClientId)]
            );
        }

        if (string.IsNullOrWhiteSpace(ClientSecret))
        {
            yield return new ValidationResult(
                "Keycloak ClientSecret is required",
                [nameof(ClientSecret)]
            );
        }

        if (RequireHttpsMetadata && !Host.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            yield return new ValidationResult(
                "SECURITY CRITICAL: RequireHttpsMetadata is enabled but Host does not use HTTPS",
                [nameof(Host)]
            );
        }
    }
}
