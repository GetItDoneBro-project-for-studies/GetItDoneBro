using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GetItDoneBro.Api.Configurations;



public static class DependencyInjection
{
    private static RsaSecurityKey? cachedRsaKey;

    public static IServiceCollection AddKeyCloakAuthentication(this IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
                {
                    var keycloakHost = configuration["KeycloakOptions:Host"] ?? throw new InvalidOperationException("KeycloakOptions:Host not configured");
                    var realm = configuration["KeycloakOptions:Realm"] ?? throw new InvalidOperationException("KeycloakOptions:Realm not configured");
                    var audience = configuration["KeycloakOptions:Audience"] ?? "account";
                    var requireHttps = bool.Parse(configuration["KeycloakOptions:RequireHttpsMetadata"] ?? "false");

                    x.MetadataAddress = $"{keycloakHost}/realms/{realm}/.well-known/openid-configuration";
                    x.RequireHttpsMetadata = requireHttps;
                    x.Authority = $"{keycloakHost}/realms/{realm}";

                    x.RefreshOnIssuerKeyNotFound = true;
                    x.AutomaticRefreshInterval = TimeSpan.FromHours(24);
                    x.SaveToken = true;

                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = $"{keycloakHost}/realms/{realm}",
                        ValidAudience = audience,
                        IssuerSigningKey = GetOrCreateRsaKey(configuration["JwtSettings:Key"] ?? throw new InvalidOperationException("No jwt key provided"), logger),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = false,
                        ClockSkew = TimeSpan.FromMinutes(2)
                    };

                    x.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            logger.LogError(
                                exception: context.Exception,
                                message: "JWT Authentication failed: {ExceptionType}",
                                context.Exception.GetType().Name
                            );
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            if (context.AuthenticateFailure != null)
                            {
                                logger.LogWarning(
                                    exception: context.AuthenticateFailure,
                                    message: "JWT Challenge failed: {FailureType}",
                                    context.AuthenticateFailure.GetType().Name
                                );
                            }

                            return HandleAuthenticationChallenge(context);
                        }
                    };
                }
            );

        return services;
    }

    private static Task HandleAuthenticationChallenge(JwtBearerChallengeContext context)
    {
        context.HandleResponse();
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var reason = DetermineSafeFailureReason(context.AuthenticateFailure);

        var result = JsonSerializer.Serialize(
            new
            {
                error = "You are not authorized",
                reason
            }
        );

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

    private static RsaSecurityKey GetOrCreateRsaKey(string publicKeyJwt, ILogger logger)
    {
        if (cachedRsaKey != null)
        {
            return cachedRsaKey;
        }

        try
        {
            var keyBytes = Convert.FromBase64String(publicKeyJwt);
            
            try
            {
                using var rsa = RSA.Create();
                rsa.ImportSubjectPublicKeyInfo(source: keyBytes, bytesRead: out _);
                cachedRsaKey = new RsaSecurityKey(rsa);
                return cachedRsaKey;
            }
            catch
            {
                using var cert = X509CertificateLoader.LoadCertificate(keyBytes);
                var publicKey = cert.GetRSAPublicKey() ?? throw new InvalidOperationException("Certificate does not contain an RSA public key");
                var rsaParams = publicKey.ExportParameters(false);
                cachedRsaKey = new RsaSecurityKey(rsaParams);
                return cachedRsaKey;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(exception: ex, message: "Invalid public key format provided");
            throw new InvalidOperationException($"Invalid public key provided: {ex.Message}", ex);
        }
    }
}
