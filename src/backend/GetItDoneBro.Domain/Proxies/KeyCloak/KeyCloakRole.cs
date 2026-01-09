namespace Adminbrothers.Portal.Domain.Proxies.KeyCloak;

public class KeyCloakRole
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool? Composite { get; set; }
    public bool? ClientRole { get; set; }
    public string? ContainerId { get; set; }
}