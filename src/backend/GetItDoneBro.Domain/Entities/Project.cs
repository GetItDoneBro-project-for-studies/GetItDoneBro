namespace GetItDoneBro.Domain.Entities;

public class Project
{
    public const int MaxDescriptionLength = 300;
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
