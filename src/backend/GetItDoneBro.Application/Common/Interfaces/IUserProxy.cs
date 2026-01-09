using GetItDoneBro.Domain.Enums;
using GetItDoneBro.Domain.Proxies.KeyCloak;

namespace GetItDoneBro.Application.Common.Interfaces;

public interface IUserProxy
{
    Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<User> GetUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> TryGetUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> AddUserAsync(KeyCloakUser user, CancellationToken cancellationToken = default);
    Task SendExecuteActionsEmailAsync(Guid userId, IEnumerable<KeyCloakUserAction> actions, CancellationToken cancellationToken = default);
    Task DisableUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
