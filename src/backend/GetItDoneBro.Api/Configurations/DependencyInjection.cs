using GetItDoneBro.Infrastructure;

namespace GetItDoneBro.Api.Configurations;

public static class DependencyInjection
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.MapDefaultEndpoints();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.ApplyMigrations();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.MapFallbackToFile("/index.html");
        return app;
    }

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.CustomConfigureAppConfiguration();
        builder.AddServiceDefaults();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddInfrastructure(builder.Configuration);
        return builder;
    }
    
    
}
