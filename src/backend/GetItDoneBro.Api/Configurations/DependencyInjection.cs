using System.Reflection;
using GetItDoneBro.Application;
using GetItDoneBro.Domain.Options;
using GetItDoneBro.Infrastructure;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace GetItDoneBro.Api.Configurations;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.CustomConfigureAppConfiguration();
        builder.AddServiceDefaults();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddInfrastructure(builder.Configuration);
        
        // Configure MediatrLicenseOptions and add Application layer services
        builder.Services.Configure<MediatrLicenseOptions>(
            builder.Configuration.GetSection(MediatrLicenseOptions.ConfigSectionPath));
        var mediatrOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<MediatrLicenseOptions>>();
        builder.Services.AddApplication(mediatrOptions);

        
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddKeycloakWebApi(builder.Configuration);

        builder.Services
            .AddAuthorization()
            .AddAuthorizationBuilder();

        builder.Services
            .AddKeycloakAuthorization()
            .AddAuthorizationServer(builder.Configuration);
        
        builder.Services.AddAuthorization();
        return builder;
    }
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.MapDefaultEndpoints();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.ApplyMigrations();
        app.UseHttpsRedirection().UseAuthentication().UseAuthorization();
        return app;
    }

    
    
}
