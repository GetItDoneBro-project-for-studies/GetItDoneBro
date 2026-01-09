using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Domain.Entities;
using GetItDoneBro.Domain.Enums;
using GetItDoneBro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.AssignUserToProject;

public interface IAssignUserToProjectHandler
{
    Task<Guid> HandleAsync(AssignUserToProjectCommand command, CancellationToken cancellationToken);
}

public sealed class AssignUserToProjectHandler(
    IRepository repository,
    IProjectUsersRepository projectUsersRepository,
    IUserResolver userResolver,
    ILogger<AssignUserToProjectHandler> logger)
    : IAssignUserToProjectHandler
{
    public async Task<Guid> HandleAsync(AssignUserToProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Assigning user {KeycloakId} to project {ProjectId} with role {Role}",
            command.KeycloakId, command.ProjectId, command.Role);

        bool projectExists = await repository.Projects.AnyAsync(p => p.Id == command.ProjectId, cancellationToken);
        if (!projectExists)
        {
            throw new ProjectNotFoundException(command.ProjectId);
        }

        var currentUserRole = await projectUsersRepository.GetUserRoleAsync(
            command.ProjectId, userResolver.UserId, cancellationToken);

        if (currentUserRole != ProjectRole.Admin)
        {
            throw new InsufficientPermissionsException("przypisywanie uzytkownikow do projektu");
        }

        if (await projectUsersRepository.IsUserAssignedAsync(command.ProjectId, Guid.Parse(command.KeycloakId), cancellationToken))
        {
            throw new UserAlreadyAssignedException(command.ProjectId, command.KeycloakId);
        }

        var projectUser = ProjectUser.Create(command.ProjectId, Guid.Parse(command.KeycloakId), command.Role);
        await repository.ProjectUsers.AddAsync(projectUser, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "User {KeycloakId} successfully assigned to project {ProjectId}. AssignmentId: {AssignmentId}",
            command.KeycloakId, command.ProjectId, projectUser.Id);

        return projectUser.Id;
    }
}
