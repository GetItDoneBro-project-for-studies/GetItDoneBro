using GetItDoneBro.Domain.Abstract;
using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Domain.Entities;

public class TaskColumn : Entity, IAuditableEntity
{
    public TaskColumn() { }

    private TaskColumn(Guid projectId, string name, int orderIndex)
    {
        ProjectId = projectId;
        Name = name;
        OrderIndex = orderIndex;
    }

    public Guid ProjectId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int OrderIndex { get; private set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    public Project Project { get; private set; } = null!;
    public ICollection<ProjectTask> Tasks { get; private set; } = new List<ProjectTask>();

    public static TaskColumn Create(Guid projectId, string name, int orderIndex)
    {
        return new TaskColumn(projectId, name, orderIndex);
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetOrderIndex(int orderIndex)
    {
        OrderIndex = orderIndex;
    }
}