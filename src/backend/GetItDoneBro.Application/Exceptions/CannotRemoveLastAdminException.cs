namespace GetItDoneBro.Application.Exceptions;

public class CannotRemoveLastAdminException : Exception
{
    public string Code { get; } = "CANNOT_REMOVE_LAST_ADMIN";
    public Guid ProjectId { get; }
    public Dictionary<string, object> Metadata { get; }

    public CannotRemoveLastAdminException(Guid projectId)
        : base($"Projekt '{projectId}' musi miec co najmniej jednego administratora.")
    {
        ProjectId = projectId;
        Metadata = new Dictionary<string, object>
        {
            { "ProjectId", projectId }
        };
    }
}