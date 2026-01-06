using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.UpdateProject;

public interface IUpdateProjectHandler
{
    Task<UpdateProjectResponse> HandleAsync(UpdateProjectRequest request, CancellationToken cancellationToken);
}

internal sealed class UpdateProjectHandler(
    IProjectsService projects,
    ILogger<UpdateProjectHandler> logger)
    : IUpdateProjectHandler
{
    public async Task<UpdateProjectResponse> HandleAsync(UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating project {ProjectId} with name {ProjectName}", request.Id, request.Name);

        var existing = await projects.GetByIdAsync(request.Id, cancellationToken);
        if (existing is null)
        {
            logger.LogWarning("Project not found for update {ProjectId}", request.Id);
            throw new ProjectNotFoundException(request.Id);
        }

        if (!string.Equals(existing.Name, request.Name, StringComparison.OrdinalIgnoreCase) &&
            await projects.NameExistsAsync(request.Name, request.Id, cancellationToken))
        {
            logger.LogWarning("Duplicate project name on update. ProjectId: {ProjectId}, Name: {ProjectName}", request.Id, request.Name);
            throw new DuplicateProjectException(request.Name);
        }

        UpdateProjectResponse response = await projects.UpdateAsync(request, cancellationToken);

        logger.LogInformation("Updated project {ProjectId}", response.Id);
        return response;
    }
}
