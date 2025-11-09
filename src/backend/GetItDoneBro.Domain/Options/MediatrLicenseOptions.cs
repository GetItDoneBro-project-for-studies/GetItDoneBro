using GetItDoneBro.Domain.Interfaces;

namespace GetItDoneBro.Domain.Options;

public sealed class MediatrLicenseOptions : IAppOptions
{
    public static string ConfigSectionPath => "MediatrLicense";
    public string LicenseKey { get; set; } = string.Empty;

    
    public bool IsValid(bool isDevelopment)
    {
        return !string.IsNullOrWhiteSpace(LicenseKey);
    }
}
