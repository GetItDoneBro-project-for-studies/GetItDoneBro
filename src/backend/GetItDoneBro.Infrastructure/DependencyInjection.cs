using System.Net.Security;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Domain.Interfaces;
using GetItDoneBro.Infrastructure.Persistence;
using GetItDoneBro.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GetItDoneBro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.RegisterOnlyInfrastructureServices();
        return services;
    }

    public static void RegisterOnlyInfrastructureServices(this IServiceCollection services)
    {
        services
            .AddHttpClient<ITokenService, TokenService>((_, client) =>
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                }
            )
            .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var isDevelopment = environment?.Equals(value: "Development", comparisonType: StringComparison.OrdinalIgnoreCase) == true;

                    var handler = new SocketsHttpHandler
                    {
                        PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                        PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                        ConnectTimeout = TimeSpan.FromSeconds(10),
                        MaxConnectionsPerServer = 50
                    };

                    if (isDevelopment)
                    {
#pragma warning disable S4830, CA5359 // Certificate validation disabled only in development
                        handler.SslOptions = new SslClientAuthenticationOptions
                        {
                            RemoteCertificateValidationCallback = (_, _, _, _) => true
                        };
#pragma warning restore S4830, CA5359
                    }

                    return handler;
                }
            );

        services
            .AddHttpClient(
                name: "KeycloakProxyClient",
                configureClient: (_, client) =>
                {
                    client.Timeout = TimeSpan.FromMinutes(5);
                }
            )
            .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var isDevelopment = environment?.Equals(value: "Development", comparisonType: StringComparison.OrdinalIgnoreCase) == true;

                    var handler = new SocketsHttpHandler
                    {
                        PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                        PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                        ConnectTimeout = TimeSpan.FromSeconds(30),
                        MaxConnectionsPerServer = 50
                    };

                    if (isDevelopment)
                    {
#pragma warning disable S4830, CA5359 // Certificate validation disabled only in development
                        handler.SslOptions = new SslClientAuthenticationOptions
                        {
                            RemoteCertificateValidationCallback = (_, _, _, _) => true
                        };
#pragma warning restore S4830, CA5359
                    }

                    return handler;
                }
            );
        
        services.AddScoped<IUserResolver, UserResolverService>();
    }

    public static IHostApplicationBuilder RegisterDatabase(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDataSource("DataBase");
        
        builder.Services.AddDbContext<GetItDoneBroDbContext>((serviceProvider, options) =>
        {
            var dataSource = serviceProvider.GetRequiredService<Npgsql.NpgsqlDataSource>();
            options.UseNpgsql(dataSource);
            options.UseSnakeCaseNamingConvention();
        });

        return builder;
    }
    
    public static WebApplication ApplyMigrations(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GetItDoneBroDbContext>();
        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }

        return webApplication;
    }
}
