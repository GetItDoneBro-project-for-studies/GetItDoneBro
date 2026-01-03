using GetItDoneBro.Api.Common;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetAllProjects;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;

namespace GetItDoneBro.Api.Features.Projects;

public class GetAllProjectsEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet("/api/projects", Handle)
            .RequireAuthorization()
            .WithTags("Projects")
            .WithName("GetAllProjects");
    }

    private static async Task<IResult> Handle(
        IGetAllProjectsHandler handler,
        CancellationToken cancellationToken)
    {
        List<ProjectDto> response = await handler.HandleAsync(cancellationToken);
        return Results.Ok(response);
    }
}
