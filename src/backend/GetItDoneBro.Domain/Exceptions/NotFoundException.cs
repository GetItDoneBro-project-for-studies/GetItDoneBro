namespace GetItDoneBro.Domain.Exceptions;

public class NotFoundException : BaseException
{
    private const int DefaultStatusCode = 404;
    private const string DefaultType = "ValidationFailure";
    private const string DefaultTitle = "Błędne dane!";
    private const string DefaultDetail = "Wystąpiły błędy walidacji.";

    public NotFoundException()
        : base(DefaultStatusCode, DefaultType, DefaultTitle, DefaultDetail)
    {
    }

    public NotFoundException(IEnumerable<ErrorDetail> errors)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, DefaultDetail, errors)
    {
    }

    public NotFoundException(string propertyName, string errorMessage)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, DefaultDetail, propertyName, errorMessage)
    {
    }
}
