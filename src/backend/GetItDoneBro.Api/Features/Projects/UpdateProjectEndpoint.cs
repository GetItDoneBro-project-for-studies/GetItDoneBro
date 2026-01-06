using FluentValidation;
using GetItDoneBro.Api.Common;
using GetItDoneBro.Api.Extensions;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Application.UseCases.Projects.Commands.UpdateProject;
using Microsoft.AspNetCore.Mvc;
using ValidationException = GetItDoneBro.Application.Exceptions.ValidationException;

namespace GetItDoneBro.Api.Features.Projects;

public class UpdateProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPut("/api/v1/projects/{id}", Handle)
            // .RequireAuthorization()
            .WithTags("Projects")
            .WithName("UpdateProject")
            .WithDescription("Updates an existing project with the provided name and description.")
            .Produces<UpdateProjectResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status499ClientClosedRequest);
    }

    private static async Task<IResult> Handle(
        [FromRoute] Guid id,
        [FromBody] UpdateProjectRequest request,
        IValidator<UpdateProjectRequest> validator,
        IUpdateProjectHandler handler,
        ILogger<UpdateProjectEndpoint> logger,
        CancellationToken cancellationToken)
    {
        // Validate ID
        if (id == Guid.Empty)
        {
            logger.LogWarning("Invalid project ID provided: {ProjectId}", id);

            return Results.BadRequest(new
            {
                message = "Project ID cannot be empty",
                code = "INVALID_PROJECT_ID"
            });
        }

        logger.LogInformation(
            "Attempting to update project. ProjectId: {ProjectId}, NewProjectName: {ProjectName}",
            id,
            request.Name);

        // Merge ID from route to request
        UpdateProjectRequest requestWithId = request with { Id = id };

        // Validate request
        IResult? validationError = await validator.ValidateRequestAsync(requestWithId, cancellationToken);
        if (validationError is not null)
        {
            logger.LogWarning("Request validation failed for update project. ProjectId: {ProjectId}", id);
            return validationError;
        }

        try
        {
            UpdateProjectResponse response = await handler.HandleAsync(requestWithId, cancellationToken);

            logger.LogInformation(
                "Project updated successfully. ProjectId: {ProjectId}, UpdatedName: {ProjectName}",
                response.Id,
                response.Name);

            return Results.Ok(response);
        }
        catch (ProjectNotFoundException ex)
        {
            logger.LogWarning(ex, "Project not found for update. ProjectId: {ProjectId}", id);

            return Results.NotFound(new
            {
                code = ex.Code,
                message = ex.Message,
                projectId = id
            });
        }
        catch (UnauthorizedProjectAccessException ex)
        {
            logger.LogWarning(ex,
                "Unauthorized access attempt to update project. ProjectId: {ProjectId}, UserId: {UserId}",
                id,
                ex.UserId);

            return Results.Forbid();
        }
        catch (DuplicateProjectException ex)
        {
            logger.LogWarning(ex,
                "Cannot update project due to duplicate name. ProjectId: {ProjectId}, ProjectName: {ProjectName}",
                id,
                request.Name);

            return Results.Conflict(new
            {
                code = ex.Code,
                message = ex.Message,
                projectName = ex.ProjectName,
                existingProjectId = ex.ExistingProjectId
            });
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex,
                "Business validation failed for update project. ProjectId: {ProjectId}, ErrorCount: {ErrorCount}",
                id,
                ex.Errors.Count);

            return Results.BadRequest(new
            {
                code = ex.Code,
                message = ex.Message,
                errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.Messages).ToArray())
            });
        }
        catch (OperationCanceledException ex)
        {
            logger.LogWarning(ex,
                "Update project request was canceled by client. ProjectId: {ProjectId}",
                id);

            return Results.StatusCode(StatusCodes.Status499ClientClosedRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error occurred while updating project. ProjectId: {ProjectId}, ExceptionType: {ExceptionType}",
                id,
                ex.GetType().Name);

            return Results.Problem(
                detail: "An internal server error occurred while updating the project",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error"
            );
        }
    }
}
