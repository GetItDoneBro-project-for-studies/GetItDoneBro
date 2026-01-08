using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Projects.Dtos;
using GetItDoneBro.Domain.Entities;
using GetItDoneBro.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;

public interface IGetProjectByIdHandler
{
    Task<ProjectDto> HandleAsync(GetProjectByIdQuery query, CancellationToken cancellationToken);
}

internal sealed class GetProjectByIdHandler(
    ILogger<GetProjectByIdHandler> logger,
    IRepository repository)
    : IGetProjectByIdHandler
{
    public async Task<ProjectDto> HandleAsync(GetProjectByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting project {ProjectId}", query.Id);

        Project project = await repository.Projects.FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken)
                          ?? throw new NotFoundException(nameof(GetProjectByIdHandler),
                              "Nie znaleziono projektu o podanym identyfikatorze!");

        logger.LogInformation("Retrieved project {ProjectId}", query.Id);

        return new ProjectDto(project.Id, project.Name, project.Description);
    }
}
