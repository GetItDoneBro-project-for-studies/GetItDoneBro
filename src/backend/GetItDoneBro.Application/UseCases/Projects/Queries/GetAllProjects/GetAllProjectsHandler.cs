using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Projects.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GetItDoneBro.Application.UseCases.Projects.Queries.GetAllProjects;

public interface IGetAllProjectsHandler
{
    Task<IEnumerable<ProjectDto>> HandleAsync(Guid? userId, CancellationToken cancellationToken);
}

public sealed class GetAllProjectsHandler(
    IRepository repository)
    : IGetAllProjectsHandler
{
    public async Task<IEnumerable<ProjectDto>> HandleAsync(Guid? userId, CancellationToken cancellationToken)
    {
        var query = repository.Projects.AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(p => p.ProjectUsers.Any(pu => pu.UserId == userId.Value));
        }

        var projects = await query.ToListAsync(cancellationToken);

        return projects.ConvertAll(x => new ProjectDto(x.Id, x.Name, x.Description));
    }
}
