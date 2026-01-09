using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Infrastructure.Persistence;
using GetItDoneBro.Infrastructure.Persistence.Interceptors;
using GetItDoneBro.Infrastructure.Repositories;
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
        services.AddScoped<IRepository>(sp => sp.GetRequiredService<GetItDoneBroDbContext>());
        services.AddScoped<IProjectsRepository, ProjectsRepository>();
        services.AddScoped<IProjectUsersRepository, ProjectUsersRepository>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<AuditableInterceptor>();
        services.AddHttpContextAccessor();
        return services;
    }

    public static IHostApplicationBuilder RegisterDatabase(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<GetItDoneBroDbContext>((provider, options) =>
        {
            AuditableInterceptor interceptor = provider.GetRequiredService<AuditableInterceptor>();
            options.UseNpgsql(builder.Configuration.GetConnectionString("DataBase"));
            options.AddInterceptors(interceptor);
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