using System.Reflection;
using FluentValidation;
using GetItDoneBro.Application.Common.Behaviors;
using GetItDoneBro.Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GetItDoneBro.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var mediatrOptions = new MediatrLicenseOptions();
        configuration.GetSection(MediatrLicenseOptions.ConfigSectionPath).Bind(mediatrOptions);
        
        services.AddMediatR(cfg =>
            {
                cfg.LicenseKey = mediatrOptions.LicenseKey;
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            }
        );
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
