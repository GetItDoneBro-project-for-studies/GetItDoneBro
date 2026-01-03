using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.Features.Projects.Commands.CreateProject;

public interface ICreateProjectHandler
{
    Task<CreateProjectResponse> HandleAsync(CreateProjectRequest request, CancellationToken cancellationToken);
}

internal sealed class CreateProjectHandler(
    ILogger<CreateProjectHandler> logger)
    : ICreateProjectHandler
{
    public async Task<CreateProjectResponse> HandleAsync(
        CreateProjectRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating project with name {ProjectName}", request.Name);

        await Task.CompletedTask;

        var projectId = Guid.NewGuid();

        logger.LogInformation("Created project with ID {ProjectId}", projectId);

        return new CreateProjectResponse(projectId, request.Name);
    }
}
