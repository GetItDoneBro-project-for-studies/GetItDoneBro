namespace GetItDoneBro.Domain.Exceptions;

public class ConfigurationException : BaseException
{
    private const int DefaultStatusCode = 500;
    private const string DefaultType = "ConfigurationError";
    private const string DefaultTitle = "Błąd konfiguracji!";
    private const string DefaultDetail = "Wystąpił błąd konfiguracji systemu.";

    public ConfigurationException(string message)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, message)
    {
    }

    public ConfigurationException(string message, Exception innerException)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, message)
    {
    }

    public ConfigurationException(string propertyName, string errorMessage)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, DefaultDetail, propertyName, errorMessage)
    {
    }

    public ConfigurationException(IEnumerable<ErrorDetail> errors)
        : base(DefaultStatusCode, DefaultType, DefaultTitle, DefaultDetail, errors)
    {
    }
}
