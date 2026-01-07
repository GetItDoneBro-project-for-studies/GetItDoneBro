using GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Queries.GetAllProjects;

public interface IGetAllProjectsHandler
{
    Task<List<ProjectDto>> HandleAsync(CancellationToken cancellationToken);
}

public sealed class GetAllProjectsHandler(
    ILogger<GetAllProjectsHandler> logger)
    : IGetAllProjectsHandler
{
    public async Task<List<ProjectDto>> HandleAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all projects");

        await Task.CompletedTask;

        logger.LogInformation("Retrieved all projects");

        return new List<ProjectDto>();
    }
}
