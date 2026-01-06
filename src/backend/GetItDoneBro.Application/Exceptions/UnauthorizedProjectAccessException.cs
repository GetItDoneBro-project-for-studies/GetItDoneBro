namespace GetItDoneBro.Application.Exceptions;

public class UnauthorizedProjectAccessException : Exception
{
    public string Code { get; } = "UNAUTHORIZED_PROJECT_ACCESS";
    public Guid ProjectId { get; }
    public Guid UserId { get; }
    public string Operation { get; }
    public Dictionary<string, object> Metadata { get; }

    public UnauthorizedProjectAccessException(
        Guid projectId,
        Guid userId,
        string operation = "access")
        : base($"User '{userId}' is not authorized to {operation} project '{projectId}'.")
    {
        ProjectId = projectId;
        UserId = userId;
        Operation = operation;
        Metadata = new Dictionary<string, object>
        {
            { "ProjectId", projectId },
            { "UserId", userId },
            { "Operation", operation }
        };
    }
}
