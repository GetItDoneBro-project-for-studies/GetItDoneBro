using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Application.UseCases.Projects.Commands.RemoveUserFromProject;
using GetItDoneBro.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.ProjectUsers.Commands.RemoveUserFromProject;

public interface IRemoveUserFromProjectHandler
{
    Task HandleAsync(RemoveUserFromProjectCommand command, CancellationToken cancellationToken);
}

public sealed class RemoveUserFromProjectHandler(
    IRepository repository,
    IProjectUsersRepository projectUsersRepository,
    ICurrentUserService currentUserService,
    ILogger<RemoveUserFromProjectHandler> logger)
    : IRemoveUserFromProjectHandler
{
    public async Task HandleAsync(RemoveUserFromProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Removing user {KeycloakId} from project {ProjectId}",
            command.KeycloakId, command.ProjectId);

        var currentUserRole = await projectUsersRepository.GetUserRoleAsync(
            command.ProjectId, currentUserService.KeycloakId!, cancellationToken);

        if (currentUserRole != ProjectRole.Admin)
        {
            throw new InsufficientPermissionsException("usuwanie uzytkownikow z projektu");
        }

        var projectUser = await projectUsersRepository.GetAsync(command.ProjectId, command.KeycloakId, cancellationToken);
        if (projectUser is null)
        {
            throw new UserNotAssignedException(command.ProjectId, command.KeycloakId);
        }

        if (projectUser.Role == ProjectRole.Admin)
        {
            int adminCount = await projectUsersRepository.GetAdminCountAsync(command.ProjectId, cancellationToken);
            if (adminCount <= 1)
            {
                throw new CannotRemoveLastAdminException(command.ProjectId);
            }
        }

        repository.ProjectUsers.Remove(projectUser);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "User {KeycloakId} successfully removed from project {ProjectId}",
            command.KeycloakId, command.ProjectId);
    }
}