using FluentValidation;
using FluentValidation.Results;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.TaskColumns.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.TaskColumns.Commands.UpdateTaskColumn;

public record UpdateTaskColumnCommand(Guid Id, string Name, int OrderIndex);

public class UpdateTaskColumnEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPut(RouteConsts.ById, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        Guid id,
        [FromBody] UpdateTaskColumnRequest request,
        [FromServices] IValidator<UpdateTaskColumnCommand> validator,
        [FromServices] IUpdateTaskColumnHandler handler,
        [FromServices] ILogger<UpdateTaskColumnEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTaskColumnCommand(id, request.Name, request.OrderIndex);

        ValidationResult? validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation(
            "Task column updated successfully. TaskColumnId: {TaskColumnId}",
            id);

        return Results.NoContent();
    }
}

public record UpdateTaskColumnRequest(string Name, int OrderIndex);
