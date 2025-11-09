using GetItDoneBro.Domain.Interfaces;
using GetItDoneBro.Infrastructure.Persistence;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GetItDoneBro.Infrastructure;

public static class DependencyInjection
{
    private const string DefaultSchema = "public";
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOnlyInfrastructureServices();
        services.RegisterDbContext(configuration);
        return services;
    }
    
    public static IServiceCollection RegisterOnlyInfrastructureServices(this IServiceCollection services)
    {

        return services;
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
    private static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<GetItDoneBroDbContext>(
            options => options
                .UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, DefaultSchema))
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IRepository>(sp => sp.GetRequiredService<GetItDoneBroDbContext>());
        
    }
}
