using System.Text.Json;
namespace GetItDoneBro.AppHost;


public static class KeycloakExtensions
{
    public static IResourceBuilder<TResource> WithKeycloakConfiguration<TResource>(
        this IResourceBuilder<TResource> builder,
        IResourceBuilder<KeycloakResource> keycloakBuilder,
        string? realmImportPath = null
        )
        where TResource : IResourceWithEnvironment
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(keycloakBuilder);

        (string realm, string clientId, string clientSecret) = TryExtractRealmDataFromImportFile(realmImportPath);

        if (string.IsNullOrWhiteSpace(realm))
        {
            throw new InvalidOperationException("Realm name could not be determined from import file.");
        }

        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new InvalidOperationException("Client ID could not be determined from import file.");
        }

        if (string.IsNullOrWhiteSpace(clientSecret))
        {
            throw new InvalidOperationException("Client secret could not be determined from import file.");
        }

        var keycloakEndpoint = keycloakBuilder.Resource.GetEndpoint("http");

        builder.WithEnvironment(
            name: "KeycloakOptions__Host",
            value: keycloakEndpoint
        );

        builder.WithEnvironment(name: "KeycloakOptions__Realm", value: realm);
        builder.WithEnvironment(name: "KeycloakOptions__ClientId", value: clientId);
        builder.WithEnvironment(name: "KeycloakOptions__ClientSecret", value: clientSecret);
        builder.WithEnvironment(name: "KeycloakOptions__Audience", value: "account");
        builder.WithEnvironment(name: "KeycloakOptions__RequireHttpsMetadata", value: "false");
        builder.WithEnvironment(name: "KeycloakOptions__SkipEmailSending", value: "false");

        return builder;
    }

    private static (string Realm, string ClientId, string ClientSecret) TryExtractRealmDataFromImportFile(string? realmImportPath)
    {
        var result = (Realm: string.Empty, ClientId: string.Empty, ClientSecret: string.Empty);

        if (string.IsNullOrWhiteSpace(realmImportPath))
        {
            return result;
        }

        try
        {
            var filePath = realmImportPath;

            if (Directory.Exists(realmImportPath))
            {
                var jsonFiles = Directory.GetFiles(path: realmImportPath, searchPattern: "*realm*.json", searchOption: SearchOption.TopDirectoryOnly);
                if (jsonFiles.Length > 0)
                {
                    filePath = jsonFiles[0];
                }
            }

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                using var document = JsonDocument.Parse(json);

                if (document.RootElement.TryGetProperty(propertyName: "realm", value: out var realmProperty))
                {
                    result.Realm = realmProperty.GetString() ?? string.Empty;
                }

                if (document.RootElement.TryGetProperty(propertyName: "clients", value: out var clientsProperty) && clientsProperty.ValueKind == JsonValueKind.Array)
                {
                    foreach (var client in clientsProperty.EnumerateArray())
                    {
                        if (client.TryGetProperty(propertyName: "clientId", value: out var clientIdProp))
                        {
                            result.ClientId = clientIdProp.GetString() ?? string.Empty;
                        }

                        if (client.TryGetProperty(propertyName: "secret", value: out var secretProp))
                        {
                            result.ClientSecret = secretProp.GetString() ?? string.Empty;
                        }

                        if (!string.IsNullOrWhiteSpace(result.ClientId) && !string.IsNullOrWhiteSpace(result.ClientSecret))
                        {
                            break;
                        }
                    }
                }
            }
        }
        catch
        {
            // Ignore errors and return empty result
        }

        return result;
    }
}

