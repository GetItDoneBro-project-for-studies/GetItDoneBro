using FluentValidation;
using FluentValidation.Results;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Projects.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(Guid Id, UpdateProjectBody Body);

public record UpdateProjectBody(string? Name, string? Description);

public class UpdateProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPut(RouteConsts.ByIdRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromRoute] Guid id,
        [FromBody] UpdateProjectBody body,
        IValidator<UpdateProjectCommand> validator,
        IUpdateProjectHandler handler,
        ILogger<UpdateProjectEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProjectCommand(id, body);

        ValidationResult? validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation(
            "Project updated successfully. ProjectId: {ProjectId}, ProjectName: {ProjectName}",
            id,
            body.Name);

        return Results.NoContent();
    }
}
