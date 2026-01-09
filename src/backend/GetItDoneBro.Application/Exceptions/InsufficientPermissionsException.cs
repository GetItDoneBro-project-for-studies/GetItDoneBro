namespace GetItDoneBro.Application.Exceptions;

public class InsufficientPermissionsException : Exception
{
    public string Code { get; } = "INSUFFICIENT_PERMISSIONS";
    public string Operation { get; }
    public Dictionary<string, object> Metadata { get; }

    public InsufficientPermissionsException(string operation)
        : base($"Nie masz uprawnien do wykonania operacji: {operation}.")
    {
        Operation = operation;
        Metadata = new Dictionary<string, object>
        {
            { "Operation", operation }
        };
    }
}