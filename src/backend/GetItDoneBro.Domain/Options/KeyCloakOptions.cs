using System.ComponentModel.DataAnnotations;
using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Domain.Options;

public class KeycloakOptions : IAppOptions, IValidatableObject
{
    public static string ConfigSectionPath => "Keycloak";

    public string AuthServerUrl { get; set; } = string.Empty;

    public string Realm { get; set; } = string.Empty;

    public string Resource { get; set; } = string.Empty;

    public string Audience { get; set; } = "account";

    public KeycloakCredentialsOptions Credentials { get; set; } = new();

    public string SslRequired { get; set; } = "none";

    public bool VerifyTokenAudience { get; set; }

    public bool SkipEmailSending { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(AuthServerUrl))
        {
            yield return new ValidationResult(
                "Keycloak AuthServerUrl is required",
                [nameof(AuthServerUrl)]
            );
        }
        else if (!Uri.IsWellFormedUriString(AuthServerUrl, UriKind.Absolute))
        {
            yield return new ValidationResult(
                "Keycloak AuthServerUrl must be a valid absolute URI",
                [nameof(AuthServerUrl)]
            );
        }

        if (string.IsNullOrWhiteSpace(Realm))
        {
            yield return new ValidationResult(
                "Keycloak Realm is required",
                [nameof(Realm)]
            );
        }

        if (string.IsNullOrWhiteSpace(Resource))
        {
            yield return new ValidationResult(
                "Keycloak Resource is required",
                [nameof(Resource)]
            );
        }

        if (string.IsNullOrWhiteSpace(Credentials.Secret))
        {
            yield return new ValidationResult(
                "Keycloak Credentials.Secret is required",
                [nameof(Credentials)]
            );
        }
    }
}

public class KeycloakCredentialsOptions
{
    public string Secret { get; set; } = string.Empty;
}
