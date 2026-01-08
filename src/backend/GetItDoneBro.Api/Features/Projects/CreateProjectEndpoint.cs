using FluentValidation;
using GetItDoneBro.Api.Common;
using GetItDoneBro.Api.Extensions;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Application.UseCases.Projects.Commands.CreateProject;
using Microsoft.AspNetCore.Mvc;
using ValidationException = GetItDoneBro.Application.Exceptions.ValidationException;

namespace GetItDoneBro.Api.Features.Projects;

public class CreateProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost("/api/v1/projects", Handle)
            .RequireAuthorization()
            .WithTags("Projects")
            .WithName("CreateProject")
            .WithDescription("Creates a new project with the provided name and description.")
            .Produces<CreateProjectResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status499ClientClosedRequest);
    }

    private static async Task<IResult> Handle(
        [FromBody] CreateProjectRequest request,
        IValidator<CreateProjectRequest> validator,
        ICreateProjectHandler handler,
        ILogger<CreateProjectEndpoint> logger,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Attempting to create project. ProjectName: {ProjectName}",
            request.Name);
        
        IResult? validationError = await validator.ValidateRequestAsync(request, cancellationToken);
        if (validationError is not null)
        {
            logger.LogWarning("Request validation failed for create project request");
            return validationError;
        }

        try
        {
            CreateProjectResponse response = await handler.HandleAsync(request, cancellationToken);

            logger.LogInformation(
                "Project created successfully. ProjectId: {ProjectId}, ProjectName: {ProjectName}",
                response.Id,
                request.Name);

            return Results.Created($"/api/v1/projects/{response.Id}", response);
        }
        catch (DuplicateProjectException ex)
        {
            logger.LogWarning(ex,
                "Cannot create project due to duplicate name. ProjectName: {ProjectName}",
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
                "Business validation failed for create project. ErrorCount: {ErrorCount}",
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
                "Create project request was canceled by client. ProjectName: {ProjectName}",
                request.Name);

            return Results.StatusCode(StatusCodes.Status499ClientClosedRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error occurred while creating project. ProjectName: {ProjectName}, ExceptionType: {ExceptionType}",
                request.Name,
                ex.GetType().Name);

            return Results.Problem(
                detail: "An internal server error occurred while creating the project",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error"
            );
        }
    }
}
