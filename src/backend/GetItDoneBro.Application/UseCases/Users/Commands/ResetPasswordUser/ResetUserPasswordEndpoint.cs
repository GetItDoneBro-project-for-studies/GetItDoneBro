using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Users.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetItDoneBro.Application.UseCases.Users.Commands.ResetPasswordUser;

public record ResetUserPasswordCommand(Guid UserId);

public class ResetUserPasswordEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPut(RouteConsts.ResetPasswordRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromRoute] Guid id,
        [FromServices] IResetUserPasswordHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new ResetUserPasswordCommand(id);
        await handler.HandleAsync(command, cancellationToken);

        return Results.NoContent();
    }
}
