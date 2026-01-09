using GetItDoneBro.Domain.Abstract;
using GetItDoneBro.Domain.Enums;
using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Domain.Entities;

public class ProjectUser : Entity, IAuditableEntity
{
    public ProjectUser() { }

    private ProjectUser(Guid projectId, string keycloakId, ProjectRole role)
    {
        ProjectId = projectId;
        KeycloakId = keycloakId;
        Role = role;
    }

    public Guid ProjectId { get; private set; }
    public string KeycloakId { get; private set; } = string.Empty;
    public ProjectRole Role { get; private set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    public Project Project { get; private set; } = null!;

    public static ProjectUser Create(Guid projectId, string keycloakId, ProjectRole role)
    {
        return new ProjectUser(projectId, keycloakId, role);
    }

    public void SetRole(ProjectRole role)
    {
        Role = role;
    }
}