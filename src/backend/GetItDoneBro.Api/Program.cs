using GetItDoneBro.Api.Configurations;

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

        builder.ConfigureServices();

        WebApplication app = builder.Build();

        app.ConfigureApplication();

        await app.RunAsync();
    }
}
