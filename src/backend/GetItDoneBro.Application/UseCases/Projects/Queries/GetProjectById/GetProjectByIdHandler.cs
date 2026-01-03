using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;

public interface IGetProjectByIdHandler
{
    Task<ProjectDto?> HandleAsync(GetProjectByIdRequest request, CancellationToken cancellationToken);
}

internal sealed class GetProjectByIdHandler(
    ILogger<GetProjectByIdHandler> logger)
    : IGetProjectByIdHandler
{
    public async Task<ProjectDto?> HandleAsync(GetProjectByIdRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting project {ProjectId}", request.Id);

        await Task.CompletedTask;

        logger.LogInformation("Retrieved project {ProjectId}", request.Id);

        return null;
    }
}
