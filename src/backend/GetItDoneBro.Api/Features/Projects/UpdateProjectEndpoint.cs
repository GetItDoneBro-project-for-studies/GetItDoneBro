using FluentValidation;
using GetItDoneBro.Api.Common;
using GetItDoneBro.Api.Extensions;
using GetItDoneBro.Application.UseCases.Projects.Commands.UpdateProject;
using Microsoft.AspNetCore.Mvc;

namespace GetItDoneBro.Api.Features.Projects;

public class UpdateProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPut("/api/v1/projects/{id}", Handle)
            // .RequireAuthorization()
            .WithTags("Projects")
            .WithName("UpdateProject");
    }

    private static async Task<IResult> Handle(
        [FromRoute] Guid id,
        [FromBody] UpdateProjectRequest request,
        IValidator<UpdateProjectRequest> validator,
        IUpdateProjectHandler handler,
        CancellationToken cancellationToken)
    {
        UpdateProjectRequest requestWithId = request with { Id = id };

        IResult? validationError = await validator.ValidateRequestAsync(requestWithId, cancellationToken);
        if (validationError is not null)
        {
            return validationError;
        }

        UpdateProjectResponse response = await handler.HandleAsync(requestWithId, cancellationToken);
        return Results.Ok(response);
    }
}
