using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Users.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetItDoneBro.Application.UseCases.Users.Commands.DisableUser;

public record DisableUserCommand(Guid UserId);

public class DisableUserEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPut(RouteConsts.DisableRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromRoute] Guid id,
        IDisableUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DisableUserCommand(id);
        await handler.HandleAsync(command, cancellationToken);

        return Results.NoContent();
    }
}
