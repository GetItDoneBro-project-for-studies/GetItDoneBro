using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.TaskColumns.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.TaskColumns.Commands.DeleteTaskColumn;

public record DeleteTaskColumnCommand(Guid Id);

public class DeleteTaskColumnEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapDelete(RouteConsts.ById, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        Guid id,
        IDeleteTaskColumnHandler handler,
        ILogger<DeleteTaskColumnEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var command = new DeleteTaskColumnCommand(id);

        await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation(
            "Task column deleted successfully. TaskColumnId: {TaskColumnId}",
            id);

        return Results.NoContent();
    }
}
