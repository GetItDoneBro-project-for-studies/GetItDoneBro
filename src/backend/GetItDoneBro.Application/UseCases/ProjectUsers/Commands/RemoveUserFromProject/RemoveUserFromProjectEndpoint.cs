using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.ProjectUsers.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.ProjectUsers.Commands.RemoveUserFromProject;

public record RemoveUserFromProjectCommand(Guid ProjectId, string KeycloakId);

public class RemoveUserFromProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapDelete(RouteConsts.ProjectUserByKeycloakIdRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromRoute] Guid projectId,
        [FromRoute] string keycloakId,
        IRemoveUserFromProjectHandler handler,
        ILogger<RemoveUserFromProjectEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var command = new RemoveUserFromProjectCommand(projectId, keycloakId);
        await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation("User {KeycloakId} removed from project {ProjectId}", keycloakId, projectId);

        return Results.NoContent();
    }
}