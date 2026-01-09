using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Application.UseCases.Projects.Shared.Dtos;
using GetItDoneBro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectUsers;

public interface IGetProjectUsersHandler
{
    Task<IEnumerable<ProjectUserDto>> HandleAsync(Guid projectId, CancellationToken cancellationToken);
}

public sealed class GetProjectUsersHandler(
    IRepository repository,
    IProjectUsersRepository projectUsersRepository,
    IUserProxy userProxy,
    IUserResolver userResolver)
    : IGetProjectUsersHandler
{
    public async Task<IEnumerable<ProjectUserDto>> HandleAsync(Guid projectId, CancellationToken cancellationToken)
    {
        bool projectExists = await repository.Projects.AnyAsync(p => p.Id == projectId, cancellationToken);
        if (!projectExists)
        {
            throw new ProjectNotFoundException(projectId);
        }
        var users = await userProxy.GetUsersAsync(cancellationToken);

        _ = await projectUsersRepository.GetUserRoleAsync(
            projectId, userResolver.UserId, cancellationToken) ?? throw new InsufficientPermissionsException("przegladanie czlonkow projektu");

        var projectUsers = await repository.ProjectUsers
            .Where(pu => pu.ProjectId == projectId)
            .ToListAsync(cancellationToken);

        var usersDict = users.ToDictionary(u => u.Id, u => u);

        return projectUsers
            .Select(pu =>
            {
                var user = usersDict.GetValueOrDefault(pu.UserId);
                return new ProjectUserDto(
                    pu.UserId,
                    user?.Username ?? string.Empty,
                    user?.FirstName,
                    user?.LastName,
                    pu.Role,
                    pu.CreatedAtUtc);
            })
            .ToList();
    }
}
