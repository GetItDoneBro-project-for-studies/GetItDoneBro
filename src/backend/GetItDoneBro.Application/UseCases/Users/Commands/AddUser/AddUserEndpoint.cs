using FluentValidation;
using FluentValidation.Results;
using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Users.Shared.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetItDoneBro.Application.UseCases.Users.Commands.AddUser;

public record AddUserCommand(
    string Email,
    string? FirstName,
    string? LastName,
    bool Enabled = true);


public class AddUserEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost(RouteConsts.BaseRoute, Handle)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        [FromBody] AddUserCommand body,
        IValidator<AddUserCommand> validator,
        IAddUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddUserCommand(body.Email, body.FirstName, body.LastName, body.Enabled);

        ValidationResult? validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        await handler.HandleAsync(command, cancellationToken);

        return Results.Created($"/api/v1/users/{body.Email}", null);
    }
}
