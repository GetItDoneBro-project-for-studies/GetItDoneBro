namespace GetItDoneBro.Application.Exceptions;

public class InsufficientPermissionsException : Exception
{
    public string Code { get; } = "INSUFFICIENT_PERMISSIONS";
    public string Operation { get; }
    public Dictionary<string, object> Metadata { get; }

    public InsufficientPermissionsException(string operation)
        : base($"You do not have permissions to perform this operation: {operation}.")
    {
        Operation = operation;
        Metadata = new Dictionary<string, object>
        {
            { "Operation", operation }
        };
    }
}
