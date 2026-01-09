using FluentValidation;
using FluentValidation.Results;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.TaskColumns.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.TaskColumns.Commands.CreateTaskColumn;

public record CreateTaskColumnCommand(Guid ProjectId, string Name, int OrderIndex);

public class CreateTaskColumnEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost(RouteConsts.BaseRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromBody] CreateTaskColumnCommand command,
        [FromServices] IValidator<CreateTaskColumnCommand> validator,
        [FromServices] ICreateTaskColumnHandler handler,
        [FromServices] ILogger<CreateTaskColumnEndpoint> logger,
        CancellationToken cancellationToken)
    {
        ValidationResult? validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        Guid response = await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation(
            "Task column created successfully. TaskColumnId: {TaskColumnId}, Name: {Name}",
            response,
            command.Name);

        return Results.Created($"/api/v1/task-columns/{response}", response);
    }
}
