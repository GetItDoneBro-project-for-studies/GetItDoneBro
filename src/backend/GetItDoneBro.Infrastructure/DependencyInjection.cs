using GetItDoneBro.Domain.Interfaces;
using GetItDoneBro.Infrastructure.Persistence;
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
        services.AddScoped<IRepository>(sp => sp.GetRequiredService<GetItDoneBroDbContext>());
        
        return services;
    }

    public static IHostApplicationBuilder RegisterDatabase(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDataSource("DataBase");
        
        builder.Services.AddDbContext<GetItDoneBroDbContext>((serviceProvider, options) =>
        {
            var dataSource = serviceProvider.GetRequiredService<Npgsql.NpgsqlDataSource>();
            options.UseNpgsql(dataSource);
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
