using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.ProjectUsers.Shared.Dtos;
using GetItDoneBro.Application.UseCases.ProjectUsers.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetItDoneBro.Application.UseCases.ProjectUsers.Queries.GetProjectUsers;

public class GetProjectUsersEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet(RouteConsts.ProjectUsersRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromRoute] Guid projectId,
        IGetProjectUsersHandler handler,
        CancellationToken cancellationToken)
    {
        IEnumerable<ProjectUserDto> result = await handler.HandleAsync(projectId, cancellationToken);
        return Results.Ok(result);
    }
}