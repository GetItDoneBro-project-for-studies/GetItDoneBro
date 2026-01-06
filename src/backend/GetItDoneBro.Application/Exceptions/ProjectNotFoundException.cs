namespace GetItDoneBro.Application.Exceptions;

public class ProjectNotFoundException : Exception
{
    public string Code { get; } = "PROJECT_NOT_FOUND";
    public Guid ProjectId { get; }
    public Dictionary<string, object> Metadata { get; }

    public ProjectNotFoundException(Guid projectId)
        : base($"Project with ID '{projectId}' was not found.")
    {
        ProjectId = projectId;
        Metadata = new Dictionary<string, object>
        {
            { "ProjectId", projectId }
        };
    }
}
