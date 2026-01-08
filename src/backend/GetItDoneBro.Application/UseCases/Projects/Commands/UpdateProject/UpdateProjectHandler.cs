using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Domain.Entities;
using GetItDoneBro.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.UpdateProject;

public interface IUpdateProjectHandler
{
    Task HandleAsync(UpdateProjectCommand request, CancellationToken cancellationToken);
}

internal sealed class UpdateProjectHandler(
    IProjectsRepository projects,
    IRepository repository,
    ILogger<UpdateProjectHandler> logger)
    : IUpdateProjectHandler
{
    public async Task HandleAsync(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating project {ProjectId} with name {ProjectName}", request.Id, request.Body.Name);

        Project project = await repository.Projects.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                          ?? throw new NotFoundException(nameof(UpdateProjectHandler),
                              "Nie znaleziono projektu o podanym identyfikatorze!");

        if (!string.IsNullOrWhiteSpace(request.Body.Name) &&
            !project.Name.Equals(request.Body.Name, StringComparison.OrdinalIgnoreCase))
        {
            if (await projects.IsNameExistsAsync(request.Body.Name, cancellationToken))
            {
                logger.LogWarning("Duplicate project name detected: {ProjectName}", request.Body.Name);
                throw new DuplicateProjectException(request.Body.Name);
            }

            project.SetName(request.Body.Name);
        }

        if (!string.IsNullOrWhiteSpace(request.Body.Description) &&
            !project.Description.Equals(request.Body.Name, StringComparison.OrdinalIgnoreCase))
        {
            project.SetDescription(request.Body.Description);
        }
    }
}
