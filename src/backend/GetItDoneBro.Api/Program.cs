using GetItDoneBro.Api.Configurations;
using GetItDoneBro.Infrastructure;
using GetItDoneBro.Infrastructure.Persistence;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GetItDoneBro.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                ApplicationName = typeof(Program).Assembly.GetName().Name
            }
        
        );
        
        var keycloakOptions = builder.Configuration.GetKeycloakOptions<KeycloakAuthenticationOptions>()!;

        Console.WriteLine(keycloakOptions.Audience);

        builder.ConfigureServices();

        WebApplication app = builder.Build();

        app.ConfigureApplication();

        await app.RunAsync();
    }
}
