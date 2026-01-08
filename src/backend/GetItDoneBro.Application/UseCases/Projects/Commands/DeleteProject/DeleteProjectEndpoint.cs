using FluentValidation;
using FluentValidation.Results;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Projects.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.DeleteProject;

public class DeleteProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapDelete(RouteConsts.ByIdRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromRoute] Guid id,
        [FromServices] IValidator<DeleteProjectCommand> validator,
        [FromServices] IDeleteProjectHandler handler,
        [FromServices] ILogger<DeleteProjectEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProjectCommand(id);
        ValidationResult? validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        await handler.HandleAsync(command, cancellationToken);
        logger.LogInformation("Successfully deleted project {ProjectId}", command.Id);

        return Results.NoContent();
    }
}
