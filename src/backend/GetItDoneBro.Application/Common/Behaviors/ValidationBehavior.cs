using FluentValidation;
using GetItDoneBro.Application.Common.Interfaces.Messaging;
using GetItDoneBro.Domain.Exceptions;
using MediatR;
using ValidationException = GetItDoneBro.Domain.Exceptions.ValidationException;

namespace GetItDoneBro.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandBase
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(context: context, cancellation: cancellationToken)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationError(
                    PropertyName: validationFailure.PropertyName,
                    ErrorMessage: validationFailure.ErrorMessage
                )
            )
            .ToList();

        if (errors.Count != 0)
        {
            throw new ValidationException(errors);
        }

        var response = await next();

        return response;
    }
}
