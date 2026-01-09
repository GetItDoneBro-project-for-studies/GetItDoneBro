using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Application.UseCases.Projects.Commands.UpdateUserRole;
using GetItDoneBro.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.ProjectUsers.Commands.UpdateUserRole;

public interface IUpdateUserRoleHandler
{
    Task HandleAsync(UpdateUserRoleCommand command, CancellationToken cancellationToken);
}

public sealed class UpdateUserRoleHandler(
    IRepository repository,
    IProjectUsersRepository projectUsersRepository,
    ICurrentUserService currentUserService,
    ILogger<UpdateUserRoleHandler> logger)
    : IUpdateUserRoleHandler
{
    public async Task HandleAsync(UpdateUserRoleCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Updating role for user {KeycloakId} in project {ProjectId} to {Role}",
            command.KeycloakId, command.ProjectId, command.Role);

        var currentUserRole = await projectUsersRepository.GetUserRoleAsync(
            command.ProjectId, currentUserService.KeycloakId!, cancellationToken);

        if (currentUserRole != ProjectRole.Admin)
        {
            throw new InsufficientPermissionsException("zmiana roli uzytkownika w projekcie");
        }

        var projectUser = await projectUsersRepository.GetAsync(command.ProjectId, command.KeycloakId, cancellationToken) ?? 
                          throw new UserNotAssignedException(command.ProjectId, command.KeycloakId);

        if (projectUser.Role == ProjectRole.Admin && command.Role != ProjectRole.Admin)
        {
            int adminCount = await projectUsersRepository.GetAdminCountAsync(command.ProjectId, cancellationToken);
            if (adminCount <= 1)
            {
                throw new CannotRemoveLastAdminException(command.ProjectId);
            }
        }

        projectUser.SetRole(command.Role);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Role for user {KeycloakId} in project {ProjectId} successfully updated to {Role}",
            command.KeycloakId, command.ProjectId, command.Role);
    }
}
