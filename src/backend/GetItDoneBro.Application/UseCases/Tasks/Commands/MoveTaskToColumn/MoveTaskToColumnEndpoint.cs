using FluentValidation;
using FluentValidation.Results;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Tasks.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Tasks.Commands.MoveTaskToColumn;

public record MoveTaskToColumnCommand(Guid TaskId, Guid TaskColumnId);

public class MoveTaskToColumnEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPatch(RouteConsts.MoveToColumn, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        Guid id,
        [FromBody] MoveTaskToColumnRequest request,
        IValidator<MoveTaskToColumnCommand> validator,
        IMoveTaskToColumnHandler handler,
        ILogger<MoveTaskToColumnEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var command = new MoveTaskToColumnCommand(id, request.TaskColumnId);

        ValidationResult? validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation(
            "Task moved successfully. TaskId: {TaskId}, NewColumnId: {TaskColumnId}",
            id,
            request.TaskColumnId);

        return Results.NoContent();
    }
}

public record MoveTaskToColumnRequest(Guid TaskColumnId);
