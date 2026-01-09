using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Domain.Proxies.KeyCloak;

namespace GetItDoneBro.Application.UseCases.Users.Commands.AddUser;

public interface IAddUserHandler
{
    Task HandleAsync(AddUserCommand command, CancellationToken cancellationToken);
}

public sealed class AddUserHandler(IUserProxy userProxy) : IAddUserHandler
{
    public async Task HandleAsync(AddUserCommand command, CancellationToken cancellationToken)
    {
        var userToCreate = KeyCloakUser.Create(
            username: command.Email,
            email: command.Email,
            firstName: command.FirstName,
            lastName: command.LastName,
            enabled: command.Enabled
        );

        await userProxy.AddUserAsync(user: userToCreate, cancellationToken: cancellationToken);
    }
}
