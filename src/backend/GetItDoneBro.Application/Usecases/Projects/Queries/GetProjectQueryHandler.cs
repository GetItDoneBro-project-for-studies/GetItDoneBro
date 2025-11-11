using GetItDoneBro.Application.Common.Interfaces.Messaging;
using GetItDoneBro.Application.Usecases.Projects.DtoS;
using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Application.Usecases.Projects.Queries;

public class GetProjectQueryHandler(IProjectRepository projectRepository) : IQueryHandler<GetProjectQuery, ProjectDto>
{
    public async Task<ProjectDto> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.Id, cancellationToken);

        if (project is null)
        {
            throw new KeyNotFoundException($"Project with id {request.Id} not found");
        }

        return new ProjectDto()
        {
            Id = project.Id,
            Name = project.Name,
            
            Description = project.Description,
            CreatedAt = project.CreatedAt,
        };
        
    }
}
