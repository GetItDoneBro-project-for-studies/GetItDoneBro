using FluentValidation;
using GetItDoneBro.Api.Common;
using GetItDoneBro.Api.Extensions;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;

namespace GetItDoneBro.Api.Features.Projects;

public class GetProjectByIdEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet("/api/v1/projects/{id}", Handle)
            .RequireAuthorization()
            .WithTags("Projects")
            .WithName("GetProjectById")
            .WithDescription("Retrieves a project by its ID.")
            .Produces<ProjectDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status499ClientClosedRequest);
    }

    private static async Task<IResult> Handle(
        Guid id,
        IValidator<GetProjectByIdRequest> validator,
        IGetProjectByIdHandler handler,
        ILogger<GetProjectByIdEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var request = new GetProjectByIdRequest(id);

        logger.LogInformation("Attempting to retrieve project. ProjectId: {ProjectId}", id);
        
        IResult? validationError = await validator.ValidateRequestAsync(request, cancellationToken);
        if (validationError is not null)
        {
            logger.LogWarning("Validation failed for get project request. ProjectId: {ProjectId}", id);
            return validationError;
        }

        try
        {
            ProjectDto response = await handler.HandleAsync(request, cancellationToken);

            logger.LogInformation("Project retrieved successfully");

            return Results.Ok(response);
        }
        catch (ProjectNotFoundException ex)
        {
            logger.LogWarning(ex, "Project not found. ProjectId: {ProjectId}", id);

            return Results.NotFound(new
            {
                code = ex.Code,
                message = ex.Message,
                projectId = id
            });
        }
        catch (OperationCanceledException ex)
        {
            logger.LogWarning(ex, "Get project by ID request was canceled by client. ProjectId: {ProjectId}", id);

            return Results.StatusCode(StatusCodes.Status499ClientClosedRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error occurred while retrieving project. ProjectId: {ProjectId}, ExceptionType: {ExceptionType}",
                id,
                ex.GetType().Name);

            return Results.Problem(
                detail: "An internal server error occurred while retrieving the project",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error"
            );
        }
    }
}
