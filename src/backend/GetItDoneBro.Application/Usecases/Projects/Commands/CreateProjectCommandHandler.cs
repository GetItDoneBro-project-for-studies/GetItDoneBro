using GetItDoneBro.Application.Common.Interfaces.Messaging;
using GetItDoneBro.Application.Usecases.Projects.DtoS;
using GetItDoneBro.Domain.Entities;
using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Application.Usecases.Projects.Commands;

public class CreateProjectCommandHandler(IProjectRepository projectRepository) : ICommandHandler<CreateProjectCommand, ProjectDto>
{
    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };
        
        await projectRepository.AddAsync(project, cancellationToken);

        return new ProjectDto()
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
        };
    }
}
