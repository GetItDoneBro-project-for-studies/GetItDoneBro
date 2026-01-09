using System.Text.Json.Serialization;

namespace Adminbrothers.Portal.Domain.Proxies.KeyCloak;

public class KeyCloakGroup
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("path")]
    public string Path { get; set; } = string.Empty;
}