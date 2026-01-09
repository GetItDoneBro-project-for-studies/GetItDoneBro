using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Application.UseCases.Projects.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GetItDoneBro.Application.UseCases.ProjectUsers.Queries.GetProjectUsers;

public interface IGetProjectUsersHandler
{
    Task<IEnumerable<ProjectUserDto>> HandleAsync(Guid projectId, CancellationToken cancellationToken);
}

public sealed class GetProjectUsersHandler(
    IRepository repository,
    IProjectUsersRepository projectUsersRepository,
    ICurrentUserService currentUserService)
    : IGetProjectUsersHandler
{
    public async Task<IEnumerable<ProjectUserDto>> HandleAsync(Guid projectId, CancellationToken cancellationToken)
    {
        bool projectExists = await repository.Projects.AnyAsync(p => p.Id == projectId, cancellationToken);
        if (!projectExists)
        {
            throw new ProjectNotFoundException(projectId);
        }

        var currentUserRole = await projectUsersRepository.GetUserRoleAsync(
            projectId, currentUserService.KeycloakId!, cancellationToken);

        if (currentUserRole is null)
        {
            throw new InsufficientPermissionsException("przegladanie czlonkow projektu");
        }

        return await repository.ProjectUsers
            .Where(pu => pu.ProjectId == projectId)
            .Select(pu => new ProjectUserDto(
                pu.Id,
                pu.KeycloakId,
                pu.Role,
                pu.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}
