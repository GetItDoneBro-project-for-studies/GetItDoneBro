using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Users.Shared.Dtos;
using GetItDoneBro.Application.UseCases.Users.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace GetItDoneBro.Application.UseCases.Users.Queries.GetUsers;

public class GetUsersEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet(RouteConsts.BaseRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        IGetUsersHandler handler,
        CancellationToken cancellationToken)
    {
        IEnumerable<UserDto> result = await handler.HandleAsync(cancellationToken);
        return Results.Ok(result);
    }
}
