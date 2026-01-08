using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GetItDoneBro.Infrastructure.Repositories;

public sealed class ProjectsRepository(GetItDoneBroDbContext dbContext) : IProjectsRepository
{
    public async Task<bool> IsNameExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects.AnyAsync(
            x => EF.Functions.ILike(x.Name, name),
            cancellationToken);
    }
}
