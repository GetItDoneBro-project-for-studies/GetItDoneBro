using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Tasks.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand(Guid Id);

public class DeleteTaskEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapDelete(RouteConsts.ById, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        Guid id,
        [FromServices] IDeleteTaskHandler handler,
        [FromServices] ILogger<DeleteTaskEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var command = new DeleteTaskCommand(id);

        await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation(
            "Task deleted successfully. TaskId: {TaskId}",
            id);

        return Results.NoContent();
    }
}
