namespace GetItDoneBro.Application.Exceptions;

public class CannotRemoveLastAdminException : Exception
{
    public string Code { get; } = "CANNOT_REMOVE_LAST_ADMIN";
    public Guid ProjectId { get; }
    public Dictionary<string, object> Metadata { get; }

    public CannotRemoveLastAdminException(Guid projectId)
        : base($"Project with ID '{projectId}' must have at least one administrator.")
    {
        ProjectId = projectId;
        Metadata = new Dictionary<string, object>
        {
            { "ProjectId", projectId }
        };
    }
}
