using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.UseCases.ProjectUsers.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GetItDoneBro.Application.UseCases.ProjectUsers.Queries.GetUserProjects;

public interface IGetUserProjectsHandler
{
    Task<IEnumerable<UserProjectDto>> HandleAsync(CancellationToken cancellationToken);
}

public sealed class GetUserProjectsHandler(
    IRepository repository,
    ICurrentUserService currentUserService)
    : IGetUserProjectsHandler
{
    public async Task<IEnumerable<UserProjectDto>> HandleAsync(CancellationToken cancellationToken)
    {
        return await repository.ProjectUsers
            .Where(pu => pu.KeycloakId == currentUserService.KeycloakId)
            .Include(pu => pu.Project)
            .Select(pu => new UserProjectDto(
                pu.ProjectId,
                pu.Project.Name,
                pu.Project.Description,
                pu.Role,
                pu.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}