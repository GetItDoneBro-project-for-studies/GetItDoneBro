using GetItDoneBro.Domain.Options;

namespace GetItDoneBro.Api.Configurations;

public static class AuthenticationConfiguration
{
    public static IServiceCollection AddKeyCloakAuthentication(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment, ILogger logger)
    {
        KeycloakOptions keycloakOptions = new();
        configuration.GetSection(KeycloakOptions.ConfigSectionPath).Bind(keycloakOptions);

        if (!keycloakOptions.IsValid(environment.IsDevelopment()))
        {
            if (environment.IsDevelopment())
            {
                logger.LogWarning(
                    "Keycloak configuration is incomplete. Realm: {Realm}, Audience: {Audience}",
                    keycloakOptions.Realm, keycloakOptions.Audience);
            }
            else
            {
                logger.LogError(
                    "Keycloak configuration is invalid for production. " +
                    "Authority: {Authority}, Realm: {Realm}, Audience: {Audience}. " +
                    "Please ensure Keycloak:Authority is set in configuration.",
                    keycloakOptions.Authority, keycloakOptions.Realm, keycloakOptions.Audience);
            }
        }

        services.Configure<KeycloakOptions>(configuration.GetSection(KeycloakOptions.ConfigSectionPath));

        services
            .AddAuthentication()
            .AddKeycloakJwtBearer(
                serviceName: "keycloak",
                realm: keycloakOptions.Realm,
                configureOptions: options =>
                {
                    options.Audience = keycloakOptions.Audience;

                    if (environment.IsDevelopment())
                    {
                        options.RequireHttpsMetadata = false;
                        logger.LogWarning(
                            "Keycloak authentication is running in Development mode with RequireHttpsMetadata=false. " +
                            "This is insecure and should never be used in production.");
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(keycloakOptions.Authority))
                        {
                            options.Authority = keycloakOptions.Authority;
                            logger.LogInformation("Keycloak Authority set to: {Authority}", keycloakOptions.Authority);
                        }
                        else
                        {
                            logger.LogError(
                                "Keycloak:Authority configuration is missing. " +
                                "Please set this to your production Keycloak server URL (e.g., https://your-keycloak-server.com/realms/{Realm})",
                                keycloakOptions.Realm);
                        }

                        options.RequireHttpsMetadata = true;
                    }
                });

        return services;
    }
}
