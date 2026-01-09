using FluentValidation;
using FluentValidation.Results;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Projects.Shared.Routes;
using GetItDoneBro.Application.UseCases.ProjectUsers.Commands.AssignUserToProject;
using GetItDoneBro.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.AssignUserToProject;

public record AssignUserToProjectCommand(Guid ProjectId, string KeycloakId, ProjectRole Role);

public record AssignUserToProjectBody(string KeycloakId, ProjectRole Role);

public class AssignUserToProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost(RouteConsts.UsersRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromRoute] Guid projectId,
        [FromBody] AssignUserToProjectBody body,
        IValidator<AssignUserToProjectCommand> validator,
        IAssignUserToProjectHandler handler,
        ILogger<AssignUserToProjectEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var command = new AssignUserToProjectCommand(projectId, body.KeycloakId, body.Role);

        ValidationResult? validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        Guid assignmentId = await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation(
            "User {KeycloakId} assigned to project {ProjectId} with role {Role}",
            body.KeycloakId, projectId, body.Role);

        return Results.Created($"/api/v1/projects/{projectId}/users/{body.KeycloakId}", assignmentId);
    }
}
