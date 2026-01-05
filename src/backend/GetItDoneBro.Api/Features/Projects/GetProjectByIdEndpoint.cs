using FluentValidation;
using GetItDoneBro.Api.Common;
using GetItDoneBro.Api.Extensions;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;

namespace GetItDoneBro.Api.Features.Projects;

public class GetProjectByIdEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet("/api/v1/projects/{id}", Handle)
            // .RequireAuthorization()
            .WithTags("Projects")
            .WithName("GetProjectById");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IValidator<GetProjectByIdRequest> validator,
        IGetProjectByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetProjectByIdRequest(id);

        IResult? validationError = await validator.ValidateRequestAsync(request, cancellationToken);
        if (validationError is not null)
        {
            return validationError;
        }

        ProjectDto? response = await handler.HandleAsync(request, cancellationToken);
        if (response is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(response);
    }
}
