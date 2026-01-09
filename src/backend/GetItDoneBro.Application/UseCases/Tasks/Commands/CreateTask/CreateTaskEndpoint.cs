using FluentValidation;
using FluentValidation.Results;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Tasks.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Tasks.Commands.CreateTask;

public record CreateTaskCommand(
    Guid ProjectId,
    Guid TaskColumnId,
    string Title,
    string Description,
    string? AssignedToKeycloakId = null,
    Uri? ImageUrl = null);

public class CreateTaskEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost(RouteConsts.BaseRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromBody] CreateTaskCommand command,
        [FromServices] IValidator<CreateTaskCommand> validator,
        [FromServices] ICreateTaskHandler handler,
        [FromServices] ILogger<CreateTaskEndpoint> logger,
        CancellationToken cancellationToken)
    {
        ValidationResult? validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        Guid response = await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation(
            "Task created successfully. TaskId: {TaskId}, Title: {Title}",
            response,
            command.Title);

        return Results.Created($"/api/v1/tasks/{response}", response);
    }
}
