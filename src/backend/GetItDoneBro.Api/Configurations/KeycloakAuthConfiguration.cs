using System.Text.Json;
using GetItDoneBro.Domain.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GetItDoneBro.Api.Configurations;

public static class KeycloakAuthConfiguration
{
    public static IServiceCollection AddKeyCloakAuthentication(
        this IServiceCollection services, 
        IOptions<KeycloakOptions> options)
    {
        var keycloakOptions = options.Value;
        
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.MetadataAddress = $"{keycloakOptions.Host}/realms/{keycloakOptions.Realm}/.well-known/openid-configuration";
                jwtBearerOptions.RequireHttpsMetadata = keycloakOptions.RequireHttpsMetadata;
                jwtBearerOptions.Authority = $"{keycloakOptions.Host}/realms/{keycloakOptions.Realm}";

                jwtBearerOptions.RefreshOnIssuerKeyNotFound = true;
                jwtBearerOptions.AutomaticRefreshInterval = TimeSpan.FromHours(1);
                jwtBearerOptions.SaveToken = true;

                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = $"{keycloakOptions.Host}/realms/{keycloakOptions.Realm}",
                    ValidAudience = keycloakOptions.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };

                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnChallenge = HandleAuthenticationChallenge
                };
            });

        return services;
    }

    private static Task HandleAuthenticationChallenge(JwtBearerChallengeContext context)
    {
        context.HandleResponse();
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var reason = DetermineSafeFailureReason(context.AuthenticateFailure);

        var result = JsonSerializer.Serialize(new
        {
            error = "You are not authorized",
            reason
        });

        return context.Response.WriteAsync(result);
    }

    private static string DetermineSafeFailureReason(Exception? failure)
    {
        return failure switch
        {
            SecurityTokenExpiredException => "Token expired",
            SecurityTokenInvalidSignatureException => "Invalid token",
            SecurityTokenInvalidIssuerException => "Invalid token issuer",
            SecurityTokenInvalidAudienceException => "Invalid token audience",
            _ => "Authentication failed"
        };
    }
}
