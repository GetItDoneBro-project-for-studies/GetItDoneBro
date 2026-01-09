using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.TaskColumns.Shared.Dtos;
using GetItDoneBro.Application.UseCases.TaskColumns.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.TaskColumns.Queries.GetTaskColumnsByProject;

public class GetTaskColumnsByProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet(RouteConsts.ByProject, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        Guid projectId,
        IGetTaskColumnsByProjectHandler handler,
        ILogger<GetTaskColumnsByProjectEndpoint> logger,
        CancellationToken cancellationToken)
    {
        IEnumerable<TaskColumnDto> taskColumns = await handler.HandleAsync(projectId, cancellationToken);

        logger.LogInformation(
            "Retrieved {Count} task columns for project {ProjectId}",
            taskColumns.Count(),
            projectId);

        return Results.Ok(taskColumns);
    }
}
