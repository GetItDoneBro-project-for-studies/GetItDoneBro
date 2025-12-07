namespace GetItDoneBro.Application.Common.Interfaces;

public interface ITokenService
{
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
    void InvalidateToken();
}
