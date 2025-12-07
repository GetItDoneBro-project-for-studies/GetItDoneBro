namespace GetItDoneBro.Application.Common.Interfaces;

public interface IUserResolver
{
    Guid GetUserId();
    bool TryGetUserId(out Guid userId);
    string GetFullName();
    bool TryGetFullName(out string fullName);
    bool GetIsEmailVerified();
    bool TryGetIsEmailVerified(out bool isEmailVerified);
    void ClearCache();
}
