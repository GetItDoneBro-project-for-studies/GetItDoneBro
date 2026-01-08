using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Projects.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GetItDoneBro.Application.UseCases.Projects.Queries.GetAllProjects;

public interface IGetAllProjectsHandler
{
    Task<IEnumerable<ProjectDto>> HandleAsync(CancellationToken cancellationToken);
}

public sealed class GetAllProjectsHandler(
    IRepository repository)
    : IGetAllProjectsHandler
{
    public async Task<IEnumerable<ProjectDto>> HandleAsync(CancellationToken cancellationToken)
    {
        return (await repository.Projects.ToListAsync(cancellationToken)).ConvertAll(x =>
            new ProjectDto(x.Id, x.Name, x.Description));
    }
}
