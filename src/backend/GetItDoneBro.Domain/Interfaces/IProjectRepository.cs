using GetItDoneBro.Domain.Entities;

namespace GetItDoneBro.Domain.Interfaces;

public interface IProjectRepository
{
    Task<Project> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Project project, CancellationToken cancellationToken);
    Task UpdateAsync(Project project, CancellationToken cancellationToken);
    Task DeleteAsync(Project project, CancellationToken cancellationToken);
}
