namespace GetItDoneBro.Domain.Exceptions;

public abstract class BaseException(
    int statusCode,
    string exceptionType,
    string title,
    string detail,
    IEnumerable<ErrorDetail>? errors = null)
    : Exception(FormatMessage(title, detail, errors))
{
    protected BaseException(
        int statusCode,
        string exceptionType,
        string title,
        string detail,
        string propertyName,
        string errorMessage)
        : this(statusCode, exceptionType, title, detail, [new ErrorDetail(propertyName, errorMessage)])
    {
    }

    public int StatusCode { get; } = statusCode;
    public string ExceptionType { get; } = exceptionType;
    public string Title { get; } = title;
    public string Detail { get; } = detail;
    public IEnumerable<ErrorDetail> Errors { get; } = errors ?? [];

    private static string FormatMessage(string title, string detail, IEnumerable<ErrorDetail>? errors)
    {
        List<ErrorDetail> errorsList = errors?.ToList() ?? [];

        if (!errorsList.Any())
        {
            return $"{title}: {detail}";
        }

        if (errorsList.Count == 1)
        {
            ErrorDetail error = errorsList[0];
            return $"{title}: [{error.PropertyName}] {error.ErrorMessage}";
        }

        IEnumerable<string> formattedErrors = errorsList.Select(e => $"[{e.PropertyName}] {e.ErrorMessage}");
        return $"{title}:{Environment.NewLine}{string.Join(Environment.NewLine, formattedErrors)}";
    }
}

public record ErrorDetail(string PropertyName, string ErrorMessage);
