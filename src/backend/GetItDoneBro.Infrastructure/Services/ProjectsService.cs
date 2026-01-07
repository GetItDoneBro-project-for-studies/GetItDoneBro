using GetItDoneBro.Application.Common.Interfaces.Services;
using GetItDoneBro.Application.UseCases.Projects.Commands.UpdateProject;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetAllProjects;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;
using GetItDoneBro.Infrastructure.Persistence;
using GetItDoneBro.Domain.Projects;
using Microsoft.EntityFrameworkCore;

namespace GetItDoneBro.Infrastructure.Services;

public sealed class ProjectsService : IProjectsService
{
    private readonly GetItDoneBroDbContext _db;

    public ProjectsService(GetItDoneBroDbContext db)
    {
        _db = db;
    }

    public Task<bool> NameExistsAsync(string name, Guid? excludeProjectId, CancellationToken cancellationToken)
    {
        var lowered = name.ToLower(System.Globalization.CultureInfo.CurrentCulture);
        return _db.Projects.AnyAsync(
            p => p.Name.Equals(lowered, StringComparison.OrdinalIgnoreCase) && (!excludeProjectId.HasValue || p.Id != excludeProjectId.Value),
            cancellationToken);
    }

    public async Task<Guid> CreateAsync(string name, string description, CancellationToken cancellationToken)
    {
        var entity = new Project
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        await _db.Projects.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var entity = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
        if (entity is null)
        {
            return false;
        }

        _db.Projects.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public Task<ProjectDto?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return _db.Projects
            .Where(p => p.Id == projectId)
            .Select(p => new ProjectDto(p.Id, p.Name, p.Description))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<List<ProjectDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _db.Projects
            .OrderBy(p => p.Name)
            .Select(p => new ProjectDto(p.Id, p.Name, p.Description))
            .ToListAsync(cancellationToken);
    }

    public async Task<UpdateProjectResponse> UpdateAsync(UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var entity = await _db.Projects.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (entity is null)
        {
            throw new InvalidOperationException("Project not found");
        }
        

        entity.Name = request.Name;
        entity.Description = request.Description ?? string.Empty;
        entity.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        return new UpdateProjectResponse(entity.Id, entity.Name);
    }
}

