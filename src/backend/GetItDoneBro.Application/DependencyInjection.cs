using System.Reflection;
using FluentValidation;
using GetItDoneBro.Application.Common.Behaviors;
using GetItDoneBro.Domain.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GetItDoneBro.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IOptions<MediatrLicenseOptions> options)
    {
        services.AddMediatR(cfg =>
            {
                cfg.LicenseKey = options.Value.LicenseKey;
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            }
        );
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
