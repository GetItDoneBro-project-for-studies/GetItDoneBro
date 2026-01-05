using GetItDoneBro.Api.Extensions;
using GetItDoneBro.Application;
using GetItDoneBro.Infrastructure;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Scalar.AspNetCore;

namespace GetItDoneBro.Api.Configurations;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.CustomConfigureAppConfiguration();
        builder.AddServiceDefaults();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddApplication();
        builder.RegisterDatabase();
        builder.Services.AddInfrastructure();

        // builder.Services
        //     .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddKeycloakWebApi(builder.Configuration);

        // builder.Services
        //     .AddAuthorization()
        //     .AddAuthorizationBuilder();

        //builder.Services
        //    .AddKeycloakAuthorization()
        //    .AddAuthorizationServer(builder.Configuration);

        // builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();

        return builder;
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.MapDefaultEndpoints();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.ApplyMigrations();
        app.UseHttpsRedirection();
        // .UseAuthentication()
        // .UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.MapEndpoints();

        return app;
    }
}
