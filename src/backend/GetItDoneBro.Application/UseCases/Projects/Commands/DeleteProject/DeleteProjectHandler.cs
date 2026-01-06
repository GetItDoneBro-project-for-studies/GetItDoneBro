using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.DeleteProject;

public interface IDeleteProjectHandler
{
    Task HandleAsync(Guid projectId, CancellationToken cancellationToken);
}

internal sealed class DeleteProjectHandler(
    IProjectsService projects,
    ILogger<DeleteProjectHandler> logger)
    : IDeleteProjectHandler
{
    public async Task HandleAsync(Guid projectId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting project {ProjectId}", projectId);

        bool deleted = await projects.DeleteAsync(projectId, cancellationToken);
        if (!deleted)
        {
            logger.LogWarning("Project not found for deletion {ProjectId}", projectId);
            throw new ProjectNotFoundException(projectId);
        }

        logger.LogInformation("Deleted project {ProjectId}", projectId);
    }
}
