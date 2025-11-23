using GetItDoneBro.Api.Configurations;
using GetItDoneBro.Infrastructure;
using GetItDoneBro.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GetItDoneBro.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                ApplicationName = typeof(Program).Assembly.GetName().Name
            }
        );
        builder.Host.CustomConfigureAppConfiguration();
        builder.AddServiceDefaults();
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks()
            .AddCheck("live", () => HealthCheckResult.Healthy(), tags: ["live"]);
        builder.Services.AddInfrastructure(builder.Configuration);

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GetItDoneBroDbContext>();
            dbContext.Database.Migrate();
        }

        app.MapDefaultEndpoints();

        app.MapHealthChecks("/api/health/live", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live")
        });

        app.UseDefaultFiles();
        app.UseStaticFiles();
        
        app.ApplyMigrations();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
