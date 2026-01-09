using FluentValidation;
using FluentValidation.Results;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Projects.Shared.Dtos;
using GetItDoneBro.Application.UseCases.Projects.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;

public record GetProjectByIdQuery(Guid Id);

public class GetProjectByIdEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet(RouteConsts.ByIdRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromRoute] Guid id,
        [FromServices] IValidator<GetProjectByIdQuery> validator,
        [FromServices] IGetProjectByIdHandler handler,
        [FromServices] ILogger<GetProjectByIdEndpoint> logger,
        CancellationToken cancellationToken)
    {
        var query = new GetProjectByIdQuery(id);
        ValidationResult? validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        ProjectDto response = await handler.HandleAsync(query, cancellationToken);

        logger.LogInformation("Project retrieved successfully");

        return Results.Ok(response);
    }
}
