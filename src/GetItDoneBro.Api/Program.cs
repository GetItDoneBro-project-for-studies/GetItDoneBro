using Scalar.AspNetCore;
using GetItDoneBro.Api.Configurations;
using GetItDoneBro.Infrastructure;
using GetItDoneBro.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
        builder.Host.CustomConfigureAppConfiguration();
        builder.AddServiceDefaults();
        builder.RegisterDatabase();
        
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddHealthChecks()
            .AddCheck("live", () => HealthCheckResult.Healthy(), tags: ["live"]);
        builder.Services.AddInfrastructure();

        WebApplication app = builder.Build();

        app.UseFileServer();

        if (app.Environment.IsDevelopment())
        {
            // Map OpenAPI and Scalar
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.MapDefaultEndpoints();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");
        
        // Apply migrations at startup
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<GetItDoneBroDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        await app.RunAsync();
    }
}
