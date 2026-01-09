using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Projects.Shared.Dtos;
using GetItDoneBro.Application.UseCases.Projects.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetItDoneBro.Application.UseCases.Projects.Queries.GetAllProjects;

public class GetAllProjectsEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet(RouteConsts.BaseRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromQuery(Name = "user_id")] Guid? userId,
        IGetAllProjectsHandler handler,
        CancellationToken cancellationToken)
    {
        IEnumerable<ProjectDto> response = await handler.HandleAsync(userId, cancellationToken);

        return Results.Ok(response);
    }
}
