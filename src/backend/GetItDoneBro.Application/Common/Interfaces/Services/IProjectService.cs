using GetItDoneBro.Application.UseCases.Projects.Commands.UpdateProject;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetAllProjects;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;

namespace GetItDoneBro.Application.Common.Interfaces.Services;

public interface IProjectsService
{
    Task<bool> NameExistsAsync(string name, Guid? excludeProjectId, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(string name, string description, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid projectId, CancellationToken cancellationToken);
    Task<ProjectDto?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken);
    Task<List<ProjectDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<UpdateProjectResponse> UpdateAsync(UpdateProjectRequest request, CancellationToken cancellationToken);
}
