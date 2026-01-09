using GetItDoneBro.Application.Common.Interfaces;

namespace GetItDoneBro.Application.UseCases.Users.Commands.DisableUser;

public interface IDisableUserHandler
{
    Task HandleAsync(DisableUserCommand command, CancellationToken cancellationToken);
}

public sealed class DisableUserHandler(IUserProxy userProxy) : IDisableUserHandler
{
    public async Task HandleAsync(DisableUserCommand command, CancellationToken cancellationToken)
    {
        await userProxy.DisableUserAsync(userId: command.UserId, cancellationToken: cancellationToken);
    }
}
