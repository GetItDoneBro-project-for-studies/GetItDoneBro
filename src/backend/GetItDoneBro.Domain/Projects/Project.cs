using GetItDoneBro.Domain.Common;

namespace GetItDoneBro.Domain.Projects;

public class Project : IEntityBase
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
