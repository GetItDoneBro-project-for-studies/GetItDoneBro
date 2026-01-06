using FluentValidation.Results;

namespace GetItDoneBro.Application.Exceptions;

public class ValidationException : Exception
{
    public string Code { get; } = "VALIDATION_FAILED";
    public IReadOnlyList<ValidationError> Errors { get; }
    public Dictionary<string, object> Metadata { get; }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation errors occurred.")
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .Select(g => new ValidationError(g.Key, g.ToList()))
            .ToList();

        Metadata = new Dictionary<string, object>
        {
            { "ValidationErrors", Errors }
        };
    }

    public class ValidationError
    {
        public string PropertyName { get; set; }
        public IReadOnlyList<string> Messages { get; set; }

        public ValidationError(string propertyName, IReadOnlyList<string> messages)
        {
            PropertyName = propertyName;
            Messages = messages;
        }
    }
}
