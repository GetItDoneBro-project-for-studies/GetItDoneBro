using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.CreateProject;

public interface ICreateProjectHandler
{
    Task<Guid> HandleAsync(CreateProjectCommand command, CancellationToken cancellationToken);
}

public sealed class CreateProjectHandler(
    IProjectsRepository projects,
    IRepository repository,
    ILogger<CreateProjectHandler> logger)
    : ICreateProjectHandler
{
    public async Task<Guid> HandleAsync(
        CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating project with name {ProjectName}", command.Name);

        if (await projects.IsNameExistsAsync(command.Name, cancellationToken))
        {
            logger.LogWarning("Duplicate project name detected: {ProjectName}", command.Name);
            throw new DuplicateProjectException(command.Name);
        }

        var project = Project.Create(command.Name, command.Description);

        await repository.Projects.AddAsync(project, cancellationToken);

        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Created project with ID {ProjectId}", project.Id);

        return project.Id;
    }
}
