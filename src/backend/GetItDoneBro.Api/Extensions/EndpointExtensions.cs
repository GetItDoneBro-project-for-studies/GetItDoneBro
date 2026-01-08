using System.Reflection;
using GetItDoneBro.Application.Common.Interfaces;

namespace GetItDoneBro.Api.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        Assembly assembly = typeof(IApiEndpoint).Assembly;

        var endpointTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } &&
                        t.IsAssignableTo(typeof(IApiEndpoint)))
            .ToList();

        foreach (Type type in endpointTypes)
        {
            var instance = Activator.CreateInstance(type) as IApiEndpoint;
            instance?.MapEndpoint(app);
        }

        return app;
    }
}
