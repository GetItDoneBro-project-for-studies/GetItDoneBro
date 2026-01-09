namespace GetItDoneBro.Domain.Proxies.KeyCloak;

public static class StringUtils
{
    public static string? NormalizeInput(string? input)
    {
        return input?.Trim();
    }
}
