using GetItDoneBro.Api.Common;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Application.UseCases.Projects.Commands.DeleteProject;

namespace GetItDoneBro.Api.Features.Projects;

public class DeleteProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapDelete("/api/v1/projects/{id}", Handle)
            .RequireAuthorization()
            .WithTags("Projects")
            .WithName("DeleteProject")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status499ClientClosedRequest);
    }

    private static async Task<IResult> Handle(
        Guid id,
        IDeleteProjectHandler handler,
        ILogger<DeleteProjectEndpoint> logger,
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            logger.LogWarning("Invalid project id provided.");
            
            return Results.BadRequest(new
            {
                message = "Project id cannot be empty"
            });
        }

        try
        {
            await handler.HandleAsync(id, cancellationToken);
            logger.LogInformation("Successfully deleted project {ProjectId}", id);  
            
            return Results.NoContent();
        }
        
        catch (ProjectNotFoundException ex)
        {
            logger.LogWarning(ex, "Project not found for deletion. ProjectId: {ProjectId}", id);

            return Results.NotFound(new
            {
                message = ex.Message,
                code = ex.Code,
                projectId = id
            });
        }
        catch (UnauthorizedProjectAccessException ex)
        {
            logger.LogWarning(ex,
                "Unauthorized access attempt to delete project. ProjectId: {ProjectId}, UserId: {UserId}",
                id,
                ex.UserId);

            return Results.Forbid();
        }
        
        catch(OperationCanceledException ex)
        {
            logger.LogWarning(ex, "Delete project operation was canceled by client");
            return Results.StatusCode(StatusCodes.Status499ClientClosedRequest);
        }
        
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error occurred while deleting project. ProjectId: {ProjectId}, ExceptionType: {ExceptionType}",
                id,
                ex.GetType().Name);

            return Results.Problem(
                detail: "An internal server error occurred while deleting the project",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error"
            );
        }
        
    }
}
