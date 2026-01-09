using GetItDoneBro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GetItDoneBro.Application.Common.Interfaces;

public interface IRepository
{
    DbSet<Project> Projects { get; }
    DbSet<ProjectUser> ProjectUsers { get; }
    DbSet<TaskColumn> TaskColumns { get; }
    DbSet<ProjectTask> ProjectTasks { get; }
    int SaveChanges();

    int SaveChanges(bool acceptAllChangesOnSuccess);

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}