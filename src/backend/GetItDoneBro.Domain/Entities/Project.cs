using GetItDoneBro.Domain.Abstract;
using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Domain.Entities;

public class Project : Entity, IAuditableEntity
{
    public Project() { }

    private Project(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    public static Project Create(string name, string description)
    {
        return new Project(name, description);
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }
}
