using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.DeleteProject;

public interface IDeleteProjectHandler
{
    Task HandleAsync(Guid projectId, CancellationToken cancellationToken);
}

internal sealed class DeleteProjectHandler(
    ILogger<DeleteProjectHandler> logger)
    : IDeleteProjectHandler
{
    public async Task HandleAsync(Guid projectId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting project {ProjectId}", projectId);


        await Task.CompletedTask;

        logger.LogInformation("Deleted project {ProjectId}", projectId);
    }
}
