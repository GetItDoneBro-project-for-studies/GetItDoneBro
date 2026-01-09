using GetItDoneBro.Domain.Abstract;
using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Domain.Entities;

public class ProjectTask : Entity, IAuditableEntity
{
    public ProjectTask() { }

    private ProjectTask(
        Guid projectId,
        Guid taskColumnId,
        string title,
        string description,
        string? assignedToKeycloakId = null,
        Uri? imageUrl = null)
    {
        ProjectId = projectId;
        TaskColumnId = taskColumnId;
        Title = title;
        Description = description;
        AssignedToKeycloakId = assignedToKeycloakId;
        ImageUrl = imageUrl;
    }

    public Guid ProjectId { get; private set; }
    public Guid TaskColumnId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? AssignedToKeycloakId { get; private set; }
    public Uri? ImageUrl { get; private set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    public Project Project { get; private set; } = null!;
    public TaskColumn TaskColumn { get; private set; } = null!;

    public static ProjectTask Create(
        Guid projectId,
        Guid taskColumnId,
        string title,
        string description,
        string? assignedToKeycloakId = null,
        Uri? imageUrl = null)
    {
        return new ProjectTask(projectId, taskColumnId, title, description, assignedToKeycloakId, imageUrl);
    }

    public void SetTitle(string title)
    {
        Title = title;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public void SetAssignedToKeycloakId(string? assignedToKeycloakId)
    {
        AssignedToKeycloakId = assignedToKeycloakId;
    }

    public void SetImageUrl(Uri? imageUrl)
    {
        ImageUrl = imageUrl;
    }

    public void MoveToColumn(Guid taskColumnId)
    {
        TaskColumnId = taskColumnId;
    }
}
