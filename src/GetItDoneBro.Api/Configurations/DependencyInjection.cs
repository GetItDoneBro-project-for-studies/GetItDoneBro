using GetItDoneBro.Domain.Options;

namespace GetItDoneBro.Api.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddKeyCloakAuthentication(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment, ILogger logger)
    {
        // Bind and validate Keycloak options
        KeycloakOptions keycloakOptions = new();
        configuration.GetSection(KeycloakOptions.ConfigSectionPath).Bind(keycloakOptions);

        // Validate configuration
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

        // Register options in DI container
        services.Configure<KeycloakOptions>(configuration.GetSection(KeycloakOptions.ConfigSectionPath));

        services
            .AddAuthentication()
            .AddKeycloakJwtBearer(
                serviceName: "keycloak",
                realm: keycloakOptions.Realm,
                configureOptions: options =>
                {
                    options.Audience = keycloakOptions.Audience;

                    // Only disable HTTPS metadata validation in development
                    // For production, explicitly set the Authority with HTTPS
                    if (environment.IsDevelopment())
                    {
                        // For development only - disable HTTPS metadata validation
                        // In production, use explicit Authority configuration instead
                        options.RequireHttpsMetadata = false;
                        logger.LogWarning(
                            "Keycloak authentication is running in Development mode with RequireHttpsMetadata=false. " +
                            "This is insecure and should never be used in production.");
                    }
                    else
                    {
                        // In production, explicitly set the Authority URL with HTTPS
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

                        // RequireHttpsMetadata = true by default (recommended for production)
                        // Explicitly setting it here for clarity
                        options.RequireHttpsMetadata = true;
                    }
                });

        return services;
    }
}
