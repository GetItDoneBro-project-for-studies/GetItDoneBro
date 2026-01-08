using FluentValidation;
using FluentValidation.Results;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Projects.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.CreateProject;

public record CreateProjectCommand(string Name, string Description);

public class CreateProjectEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost(RouteConsts.BaseRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromBody] CreateProjectCommand command,
        IValidator<CreateProjectCommand> validator,
        ICreateProjectHandler handler,
        ILogger<CreateProjectEndpoint> logger,
        CancellationToken cancellationToken)
    {
        ValidationResult? validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        Guid response = await handler.HandleAsync(command, cancellationToken);

        logger.LogInformation(
            "Project created successfully. ProjectId: {ProjectId}, ProjectName: {ProjectName}",
            response,
            command.Name);

        return Results.Created($"/api/v1/projects/{response}", response);
    }
}
