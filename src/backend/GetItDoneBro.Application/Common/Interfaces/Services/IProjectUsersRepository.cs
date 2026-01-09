using GetItDoneBro.Domain.Entities;
using GetItDoneBro.Domain.Enums;

namespace GetItDoneBro.Application.Common.Interfaces.Services;

public interface IProjectUsersRepository
{
    Task<ProjectUser?> GetAsync(Guid projectId, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsUserAssignedAsync(Guid projectId, Guid userId, CancellationToken cancellationToken = default);
    Task<ProjectRole?> GetUserRoleAsync(Guid projectId, Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetAdminCountAsync(Guid projectId, CancellationToken cancellationToken = default);
}