using FluentValidation;
using FluentValidation.Results;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Projects.Shared.Routes;
using GetItDoneBro.Application.UseCases.ProjectUsers.Commands.UpdateUserRole;
using GetItDoneBro.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.UpdateUserRole;

public record UpdateUserRoleCommand(Guid ProjectId, string KeycloakId, ProjectRole Role);

public record UpdateUserRoleBody(ProjectRole Role);

public class UpdateUserRoleEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPut(RouteConsts.UserByIdRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromRoute] Guid projectId,
        [FromRoute] string userId,
        [FromBody] UpdateUserRoleBody body,
        IValidator<UpdateUserRoleCommand> validator,
        IUpdateUserRoleHandler handler,
        ILogger<UpdateUserRoleEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserRoleCommand(projectId, userId, body.Role);

        ValidationResult? validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation(
            "User {UserId} role updated to {Role} in project {ProjectId}",
            userId, body.Role, projectId);

        return Results.NoContent();
    }
}
