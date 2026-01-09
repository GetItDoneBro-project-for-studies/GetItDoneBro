using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Domain.Enums;

namespace GetItDoneBro.Application.UseCases.Users.Commands.ResetPasswordUser;

public interface IResetUserPasswordHandler
{
    Task HandleAsync(ResetUserPasswordCommand command, CancellationToken cancellationToken);
}

public sealed class ResetUserPasswordHandler(IUserProxy userProxy) : IResetUserPasswordHandler
{
    public async Task HandleAsync(ResetUserPasswordCommand command, CancellationToken cancellationToken)
    {
        await userProxy.SendExecuteActionsEmailAsync(userId: command.UserId, actions: [KeyCloakUserAction.UPDATE_PASSWORD], cancellationToken: cancellationToken);
    }
}
