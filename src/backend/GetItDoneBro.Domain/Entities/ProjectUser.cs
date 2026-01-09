using GetItDoneBro.Domain.Abstract;
using GetItDoneBro.Domain.Enums;
using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Domain.Entities;

public class ProjectUser : Entity, IAuditableEntity
{
    public ProjectUser() { }

    private ProjectUser(Guid projectId, Guid userId, ProjectRole role)
    {
        ProjectId = projectId;
        UserId = userId;
        Role = role;
    }

    public Guid ProjectId { get; private set; }
    public Guid UserId { get; private set; }
    public ProjectRole Role { get; private set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    public Project Project { get; private set; } = null!;

    public static ProjectUser Create(Guid projectId, Guid userId, ProjectRole role)
    {
        return new ProjectUser(projectId, userId, role);
    }

    public void SetRole(ProjectRole role)
    {
        Role = role;
    }
}
