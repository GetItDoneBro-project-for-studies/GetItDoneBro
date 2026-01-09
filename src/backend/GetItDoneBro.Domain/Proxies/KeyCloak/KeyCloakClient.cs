namespace GetItDoneBro.Domain.Proxies.KeyCloak;

public class KeyCloakClient
{
    public string? Id { get; set; }
    public string? ClientId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool Enabled { get; set; } = true;
    public string? RootUrl { get; set; }
    public string? BaseUrl { get; set; }
    public string? AdminUrl { get; set; }
    public string? Protocol { get; set; }
    public string? ClientAuthenticatorType { get; set; }
    public bool PublicClient { get; set; }
    public bool ServiceAccountsEnabled { get; set; }
    public string? Secret { get; set; }
    public bool BearerOnly { get; set; }
    public bool DirectAccessGrantsEnabled { get; set; }
    public List<string>? RedirectUris { get; set; }
    public List<string>? WebOrigins { get; set; }
    public Dictionary<string, object>? Attributes { get; set; }

    public static KeyCloakClient Create(
        string clientId,
        string? name = null,
        string? description = null,
        bool enabled = true,
        bool serviceAccountsEnabled = false,
        bool publicClient = false,
        List<string>? redirectUris = null,
        List<string>? webOrigins = null,
        Dictionary<string, object>? attributes = null
        )
    {
        return new KeyCloakClient
        {
            ClientId = clientId,
            Name = name ?? clientId,
            Description = description,
            Enabled = enabled,
            ServiceAccountsEnabled = serviceAccountsEnabled,
            PublicClient = publicClient,
            RedirectUris = redirectUris ?? [],
            WebOrigins = webOrigins ?? [],
            Attributes = attributes ?? new Dictionary<string, object>()
        };
    }
}
