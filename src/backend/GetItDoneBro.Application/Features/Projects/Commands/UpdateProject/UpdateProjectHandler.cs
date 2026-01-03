using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.Features.Projects.Commands.UpdateProject;

public interface IUpdateProjectHandler
{
    Task<UpdateProjectResponse> HandleAsync(UpdateProjectRequest request, CancellationToken cancellationToken);
}

internal sealed class UpdateProjectHandler(
    ILogger<UpdateProjectHandler> logger)
    : IUpdateProjectHandler
{
    public async Task<UpdateProjectResponse> HandleAsync(
        UpdateProjectRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating project {ProjectId} with name {ProjectName}", request.Id, request.Name);

        await Task.CompletedTask;

        logger.LogInformation("Updated project {ProjectId}", request.Id);

        return new UpdateProjectResponse(request.Id, request.Name);
    }
}
