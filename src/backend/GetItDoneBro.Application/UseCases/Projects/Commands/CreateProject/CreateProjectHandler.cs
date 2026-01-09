using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Domain.Entities;
using GetItDoneBro.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.CreateProject;

public interface ICreateProjectHandler
{
    Task<Guid> HandleAsync(CreateProjectCommand command, CancellationToken cancellationToken);
}

public sealed class CreateProjectHandler(
    IProjectsRepository projects,
    IRepository repository,
    ICurrentUserService currentUserService,
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

        var projectUser = ProjectUser.Create(
            project.Id,
            currentUserService.KeycloakId!,
            ProjectRole.Admin);
        await repository.ProjectUsers.AddAsync(projectUser, cancellationToken);

        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Created project with ID {ProjectId}, creator {KeycloakId} assigned as Admin",
            project.Id, currentUserService.KeycloakId);

        return project.Id;
    }
}