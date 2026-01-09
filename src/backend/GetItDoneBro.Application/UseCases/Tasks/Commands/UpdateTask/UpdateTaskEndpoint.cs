using FluentValidation;
using FluentValidation.Results;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Tasks.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(
    Guid Id,
    string Title,
    string Description,
    string? AssignedToKeycloakId = null,
    Uri? ImageUrl = null);

public class UpdateTaskEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPut(RouteConsts.ById, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        Guid id,
        [FromBody] UpdateTaskRequest request,
        [FromServices] IValidator<UpdateTaskCommand> validator,
        [FromServices] IUpdateTaskHandler handler,
        [FromServices] ILogger<UpdateTaskEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTaskCommand(
            id,
            request.Title,
            request.Description,
            request.AssignedToKeycloakId,
            request.ImageUrl);

        ValidationResult? validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation(
            "Task updated successfully. TaskId: {TaskId}",
            id);

        return Results.NoContent();
    }
}

public record UpdateTaskRequest(
    string Title,
    string Description,
    string? AssignedToKeycloakId = null,
    Uri? ImageUrl = null);
