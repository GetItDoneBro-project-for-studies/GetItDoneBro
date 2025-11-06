namespace GetItDoneBro.Domain.Models;

public class AuditableEntity
{
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
