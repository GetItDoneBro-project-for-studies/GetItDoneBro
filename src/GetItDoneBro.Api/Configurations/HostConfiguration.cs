using System.Reflection;
using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Api.Configurations;

public static class HostConfiguration
{
    public static void CustomConfigureAppConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((context, config) =>
            {
                IHostEnvironment env = context.HostingEnvironment;

                config
                    .AddJsonFile(path: "appsettings.json", optional: false)
                    .AddJsonFile(path: $"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();
            }
        );
    }

    public static void ConfigureOptions(this IServiceCollection services)
    {
        var optionTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false }
                           && typeof(IAppOptions).IsAssignableFrom(type))
            .ToList();

        foreach (Type optionType in optionTypes)
        {
            typeof(HostConfiguration)
                .GetMethod(nameof(AddOptionsWithValidation))!
                .MakeGenericMethod(optionType)
                .Invoke(null, [services]);
        }
    }
    public static IServiceCollection AddOptionsWithValidation<TOptions>(this IServiceCollection services)
        where TOptions : class, IAppOptions
    {
        return services
            .AddOptions<TOptions>()
            .BindConfiguration(configSectionPath: TOptions.ConfigSectionPath,
                configureBinder: binderOptions => binderOptions.BindNonPublicProperties = true)
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Services;
    }
}
