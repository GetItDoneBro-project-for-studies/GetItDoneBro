namespace GetItDoneBro.Domain.Interfaces;

public interface IUserResolver
{
    Guid UserId { get; }
    string FullName { get; }
    bool IsEmailVerified { get; }
}
