namespace GetItDoneBro.Domain.Exceptions;

public class IntegrityException : BaseException
{
    private const int DefaultStatusCode = 422;
    private const string DefaultType = "IntegrityError";
    private const string DefaultTitle = "Błąd integralności danych!";
    private const string DefaultDetail = "Wystąpiły błędy związane z integralnością danych.";

    public IntegrityException(string message)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, message)
    {
        EntityType = string.Empty;
    }

    public IntegrityException(string message, Exception inner)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, message)
    {
        EntityType = string.Empty;
    }

    public IntegrityException(string propertyName, string errorMessage)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, DefaultDetail, propertyName, errorMessage)
    {
        EntityType = string.Empty;
    }

    public Guid? EntityId { get; set; }
    public string EntityType { get; set; }
}
