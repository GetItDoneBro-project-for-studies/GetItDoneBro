using GetItDoneBro.Api.Common;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetAllProjects;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;
using Microsoft.AspNetCore.Mvc;

namespace GetItDoneBro.Api.Features.Projects;

public class GetAllProjectsEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet("/api/v1/projects", Handle)
            // .RequireAuthorization()
            .WithTags("Projects")
            .WithName("GetAllProjects")
            .Produces<IEnumerable<ProjectDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status499ClientClosedRequest);
    }

    private static async Task<IResult> Handle(
        IGetAllProjectsHandler handler,
        ILogger<GetAllProjectsEndpoint> logger,
        CancellationToken cancellationToken)
    {
        try
        {
            List<ProjectDto> response = await handler.HandleAsync(cancellationToken);
            logger.LogInformation("Successfully retrieved {ProjectCount} projects", response.Count);
            return Results.Ok(response);
        }

        catch (OperationCanceledException ex)
        {
            logger.LogWarning(ex, "Get all projects operation was canceled by client");
            return Results.StatusCode(StatusCodes.Status499ClientClosedRequest);
        }
        
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get all projects. Ex type: {ExceptionType}, Message: {ExceptionMessage}", ex.GetType().Name, ex.Message);
            return Results.Problem(
                detail: "Internal Server error while getting all projects", 
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error"
                );
        }
    }
}
