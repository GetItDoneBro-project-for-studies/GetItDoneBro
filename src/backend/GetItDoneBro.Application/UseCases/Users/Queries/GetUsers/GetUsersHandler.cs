using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Users.Shared.Dtos;

namespace GetItDoneBro.Application.UseCases.Users.Queries.GetUsers;

public interface IGetUsersHandler
{
    Task<IEnumerable<UserDto>> HandleAsync(CancellationToken cancellationToken);
}

public sealed class GetUsersHandler(IUserProxy userProxy) : IGetUsersHandler
{
    public async Task<IEnumerable<UserDto>> HandleAsync(CancellationToken cancellationToken)
    {
        var users = (await userProxy.GetUsersAsync(cancellationToken)).Select(user => new UserDto(user));

        return users;
    }
}
