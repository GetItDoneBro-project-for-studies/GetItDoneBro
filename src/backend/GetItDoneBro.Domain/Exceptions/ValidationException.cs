namespace GetItDoneBro.Domain.Exceptions;

public class ValidationException : BaseException
{
    private const int DefaultStatusCode = 400;
    private const string DefaultType = "ValidationFailure";
    private const string DefaultTitle = "Błędne dane!";
    private const string DefaultDetail = "Wystąpiły błędy walidacji.";

    public ValidationException()
        : base(DefaultStatusCode, DefaultType, DefaultTitle, DefaultDetail)
    {
    }

    public ValidationException(IEnumerable<ErrorDetail> errors)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, DefaultDetail, errors)
    {
    }

    public ValidationException(string propertyName, string errorMessage)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, DefaultDetail, propertyName, errorMessage)
    {
    }
}
