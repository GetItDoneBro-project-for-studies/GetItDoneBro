using GetItDoneBro.Domain.Entities;
using GetItDoneBro.Domain.Interfaces;
using GetItDoneBro.Infrastructure.Persistence;

namespace GetItDoneBro.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly GetItDoneBroDbContext context;

    public ProjectRepository(GetItDoneBroDbContext context)
    {
        this.context = context;
    }
    
    public async Task<Project> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var task = await context.FindAsync<Project>(new object[] { id }, cancellationToken);
        if (task is null)
        {
            throw new ArgumentNullException($"Project with id {id} not found");
        }
        
        return task;
    }

    public async Task AddAsync(Project project, CancellationToken cancellationToken)
    {
        await context.AddAsync(project, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Project project, CancellationToken cancellationToken)
    {
        context.Update(project);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Project project, CancellationToken cancellationToken)
    {
        context.Remove(project);
        await context.SaveChangesAsync(cancellationToken);
    }
}
