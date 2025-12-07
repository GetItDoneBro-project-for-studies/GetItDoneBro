using System.ComponentModel.DataAnnotations;
using System.Reflection;
using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Api.Configurations;

public static class OptionsConfiguration
{
    public static void ConfigureOptions(this WebApplicationBuilder builder)
    {
        var skipValidation = IsTestingEnvironment();

        var optionTypes = GetOptionTypes();

        foreach (var optionType in optionTypes)
        {
            RegisterOption(builder.Services, optionType, skipValidation);
        }
    }

    private static bool IsTestingEnvironment()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return environment?.Equals("Testing", StringComparison.OrdinalIgnoreCase) == true;
    }

    private static List<Type> GetOptionTypes()
    {
        return typeof(IAppOptions).Assembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false }
                           && typeof(IAppOptions).IsAssignableFrom(type)
                           && typeof(IValidatableObject).IsAssignableFrom(type))
            .ToList();
    }

    private static void RegisterOption(IServiceCollection services, Type optionType, bool skipValidation)
    {
        typeof(OptionsConfiguration)
#pragma warning disable S3011
            .GetMethod(nameof(ConfigureOption), BindingFlags.NonPublic | BindingFlags.Static)!
#pragma warning restore S3011
            .MakeGenericMethod(optionType)
            .Invoke(null, [services, skipValidation]);
    }

    private static void ConfigureOption<TOptions>(IServiceCollection services, bool skipValidation)
        where TOptions : class, IAppOptions, IValidatableObject, new()
    {
        var optionsBuilder = services
            .AddOptions<TOptions>()
            .BindConfiguration(TOptions.ConfigSectionPath, o => o.BindNonPublicProperties = true);

        if (!skipValidation)
        {
            optionsBuilder
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}
