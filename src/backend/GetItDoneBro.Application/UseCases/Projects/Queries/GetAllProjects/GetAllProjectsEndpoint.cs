using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Projects.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace GetItDoneBro.Application.UseCases.Projects.Queries.GetAllProjects;

public class GetAllProjectsEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet("/api/v1/projects", Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        IGetAllProjectsHandler handler,
        CancellationToken cancellationToken)
    {
        IEnumerable<ProjectDto> response = await handler.HandleAsync(cancellationToken);

        return Results.Ok(response);
    }
}
