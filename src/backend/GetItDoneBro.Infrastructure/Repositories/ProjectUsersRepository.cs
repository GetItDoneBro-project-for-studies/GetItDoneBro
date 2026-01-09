using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Domain.Entities;
using GetItDoneBro.Domain.Enums;
using GetItDoneBro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GetItDoneBro.Infrastructure.Repositories;

public sealed class ProjectUsersRepository(GetItDoneBroDbContext dbContext) : IProjectUsersRepository
{
    public async Task<ProjectUser?> GetAsync(
        Guid projectId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectUsers
            .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId, cancellationToken);
    }

    public async Task<bool> IsUserAssignedAsync(
        Guid projectId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectUsers
            .AnyAsync(pu => pu.ProjectId == projectId && pu.UserId == userId, cancellationToken);
    }

    public async Task<ProjectRole?> GetUserRoleAsync(
        Guid projectId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var projectUser = await dbContext.ProjectUsers
            .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId, cancellationToken);

        return projectUser?.Role;
    }

    public async Task<int> GetAdminCountAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProjectUsers
            .CountAsync(pu => pu.ProjectId == projectId && pu.Role == ProjectRole.Admin, cancellationToken);
    }
}