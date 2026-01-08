using GetItDoneBro.Api.Common;
using GetItDoneBro.Application.UseCases.Projects.Commands.DeleteProject;

namespace GetItDoneBro.Api.Features.Projects;

public class DeleteProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapDelete("/api/v1/projects/{id}", Handle)
            .RequireAuthorization()
            .WithTags("Projects")
            .WithName("DeleteProject");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IDeleteProjectHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(id, cancellationToken);
        return Results.NoContent();
    }
}
