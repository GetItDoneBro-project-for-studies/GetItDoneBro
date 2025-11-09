namespace GetItDoneBro.Domain.Exceptions;


public class ValidationException : Exception
{
    public ValidationException()
    {
        Errors = [];
    }

    public ValidationException(IEnumerable<ValidationError> errors)
        : base(FormatErrorMessage(errors))
    {
        Errors = errors;
    }

    public ValidationException(string propertyName, string errorMessage)
        : base($"[{propertyName}] {errorMessage}")
    {
        Errors = [new ValidationError(PropertyName: propertyName, ErrorMessage: errorMessage)];
    }

    public IEnumerable<ValidationError> Errors { get; }

    private static string FormatErrorMessage(IEnumerable<ValidationError> errors)
    {
        var errorsList = errors.ToList();
        switch (errorsList.Count)
        {
            case 0:
                return "One or more validation failures have occurred.";
            case 1:
                {
                    var error = errorsList[0];
                    return $"[{error.PropertyName}] {error.ErrorMessage}";
                }
            default:
                {
                    var formattedErrors = errorsList.Select(e => $"[{e.PropertyName}] {e.ErrorMessage}");
                    return $"Validation failures:{Environment.NewLine}{string.Join(separator: Environment.NewLine, values: formattedErrors)}";
                }
        }
    }
}

public record ValidationError(string PropertyName, string ErrorMessage);
