using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Tasks.Shared.Dtos;
using GetItDoneBro.Application.UseCases.Tasks.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Tasks.Queries.GetTasksByProject;

public class GetTasksByProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet(RouteConsts.ByProject, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        Guid projectId,
        IGetTasksByProjectHandler handler,
        ILogger<GetTasksByProjectEndpoint> logger,
        CancellationToken cancellationToken)
    {
        IEnumerable<TaskDto> tasks = await handler.HandleAsync(projectId, cancellationToken);

        logger.LogInformation(
            "Retrieved {Count} tasks for project {ProjectId}",
            tasks.Count(),
            projectId);

        return Results.Ok(tasks);
    }
}
