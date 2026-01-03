using FluentValidation;
using GetItDoneBro.Api.Common;
using GetItDoneBro.Api.Extensions;
using GetItDoneBro.Application.UseCases.Projects.Commands.CreateProject;
using Microsoft.AspNetCore.Mvc;

namespace GetItDoneBro.Api.Features.Projects;

public class CreateProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost("/api/projects", Handle)
            .RequireAuthorization()
            .WithTags("Projects")
            .WithName("CreateProject");
    }

    private static async Task<IResult> Handle(
        [FromBody] CreateProjectRequest request,
        IValidator<CreateProjectRequest> validator,
        ICreateProjectHandler handler,
        CancellationToken cancellationToken)
    {
        IResult? validationError = await validator.ValidateRequestAsync(request, cancellationToken);
        if (validationError is not null)
        {
            return validationError;
        }

        CreateProjectResponse response = await handler.HandleAsync(request, cancellationToken);
        return Results.Created($"/api/projects/{response.Id}", response);
    }
}
