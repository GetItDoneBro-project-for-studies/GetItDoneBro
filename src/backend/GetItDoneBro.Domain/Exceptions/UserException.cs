namespace GetItDoneBro.Domain.Exceptions;

public class UserException : BaseException
{
    private const int DefaultStatusCode = 400;
    private const string DefaultType = "UserFailure";
    private const string DefaultTitle = "Błąd użytkownika!";
    private const string DefaultDetail = "Wystąpiły błędy związane z użytkownikiem.";

    public UserException()
        : base(DefaultStatusCode, DefaultType, DefaultTitle, DefaultDetail)
    {
    }

    public UserException(IEnumerable<ErrorDetail> errors)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, DefaultDetail, errors)
    {
    }

    public UserException(string propertyName, string errorMessage)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, DefaultDetail, propertyName, errorMessage)
    {
    }
}
