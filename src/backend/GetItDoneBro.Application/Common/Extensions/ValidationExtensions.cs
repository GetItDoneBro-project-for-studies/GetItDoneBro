using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace GetItDoneBro.Application.Common.Extensions;

public static class ValidationExtensions
{
    public static async Task<IResult?> ValidateRequestAsync<T>(
        this IValidator<T> validator,
        T request,
        CancellationToken cancellationToken)
    {
        ValidationResult? validationResult = await validator.ValidateAsync(request, cancellationToken);
        return !validationResult.IsValid
            ? Results.ValidationProblem(validationResult.ToDictionary())
            : null;
    }
}
