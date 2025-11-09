using GetItDoneBro.Api.Configurations;
using GetItDoneBro.Infrastructure;
using GetItDoneBro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

        await app.RunAsync();
    }
}
