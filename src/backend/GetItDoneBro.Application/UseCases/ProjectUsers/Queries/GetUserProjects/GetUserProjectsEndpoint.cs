using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.ProjectUsers.Shared.Dtos;
using GetItDoneBro.Application.UseCases.ProjectUsers.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace GetItDoneBro.Application.UseCases.ProjectUsers.Queries.GetUserProjects;

public class GetUserProjectsEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet(RouteConsts.CurrentUserProjectsRoute, HandleCurrentUser)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleCurrentUser(
        IGetUserProjectsHandler handler,
        CancellationToken cancellationToken)
    {
        IEnumerable<UserProjectDto> result = await handler.HandleAsync(cancellationToken);
        return Results.Ok(result);
    }
}