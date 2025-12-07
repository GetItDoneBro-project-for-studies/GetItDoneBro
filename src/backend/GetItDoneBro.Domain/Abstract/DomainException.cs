using GetItDoneBro.Domain.Exceptions;

namespace GetItDoneBro.Domain.Abstract;

public abstract class DomainException : Exception
{
    protected DomainException(
        int statusCode,
        string typeError,
        string title,
        string detail,
        IEnumerable<ErrorDetail>? errors = null)
        : base(FormatMessage(title, detail, errors))
    {
        StatusCode = statusCode;
        TypeError = typeError;
        Title = title;
        Detail = detail;
        Errors = errors ?? [];
    }

    protected DomainException(
        int statusCode,
        string typeError,
        string title,
        string detail,
        string propertyName,
        string errorMessage)
        : this(statusCode, typeError, title, detail, [new ErrorDetail(propertyName, errorMessage)])
    {
    }

    public int StatusCode { get; }
    public string TypeError { get; }
    public string Title { get; }
    public string Detail { get; }
    public IEnumerable<ErrorDetail> Errors { get; }

    private static string FormatMessage(string title, string detail, IEnumerable<ErrorDetail>? errors)
    {
        var errorDetails = errors as ErrorDetail[] ?? errors!.ToArray();
        if (errors is null || !errorDetails.Any())
        {
            return $"{title}: {detail}";
        }

        var errorsList = errorDetails.ToList();
        if (errorsList.Count == 1)
        {
            var error = errorsList[0];
            return $"{title}: [{error.PropertyName}] {error.ErrorMessage}";
        }

        var formattedErrors = errorsList.Select(e => $"[{e.PropertyName}] {e.ErrorMessage}");
        return $"{title}:{Environment.NewLine}{string.Join(Environment.NewLine, formattedErrors)}";
    }
}
