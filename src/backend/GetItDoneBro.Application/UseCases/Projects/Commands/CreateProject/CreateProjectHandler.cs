// csharp
using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.CreateProject;

public interface ICreateProjectHandler
{
    Task<CreateProjectResponse> HandleAsync(CreateProjectRequest request, CancellationToken cancellationToken);
}

internal sealed class CreateProjectHandler(
    IProjectsService projects,
    ILogger<CreateProjectHandler> logger)
    : ICreateProjectHandler
{
    public async Task<CreateProjectResponse> HandleAsync(
        CreateProjectRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating project with name {ProjectName}", request.Name);

        if (await projects.NameExistsAsync(request.Name, null, cancellationToken))
        {
            logger.LogWarning("Duplicate project name detected: {ProjectName}", request.Name);
            throw new DuplicateProjectException(request.Name);
        }

        Guid projectId = await projects.CreateAsync(request.Name, request.Description, cancellationToken);

        logger.LogInformation("Created project with ID {ProjectId}", projectId);

        return new CreateProjectResponse(projectId, request.Name);
    }
}
