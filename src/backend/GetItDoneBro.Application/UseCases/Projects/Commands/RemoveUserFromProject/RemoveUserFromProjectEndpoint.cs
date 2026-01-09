using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Projects.Shared.Routes;
using GetItDoneBro.Application.UseCases.ProjectUsers.Commands.RemoveUserFromProject;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.RemoveUserFromProject;

public record RemoveUserFromProjectCommand(Guid ProjectId, string KeycloakId);

public class RemoveUserFromProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapDelete(RouteConsts.UserByIdRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromRoute] Guid projectId,
        [FromRoute] string userId,
        IRemoveUserFromProjectHandler handler,
        ILogger<RemoveUserFromProjectEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var command = new RemoveUserFromProjectCommand(projectId, userId);
        await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation("User {UserId} removed from project {ProjectId}", userId, projectId);

        return Results.NoContent();
    }
}
