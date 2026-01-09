namespace GetItDoneBro.Application.Exceptions;

public class UserAlreadyAssignedException : Exception
{
    public string Code { get; } = "USER_ALREADY_ASSIGNED";
    public Guid ProjectId { get; }
    public string KeycloakId { get; }
    public Dictionary<string, object> Metadata { get; }

    public UserAlreadyAssignedException(Guid projectId, string keycloakId)
        : base($"User with ID '{keycloakId}' is already assigned to project with ID '{projectId}'.")
    {
        ProjectId = projectId;
        KeycloakId = keycloakId;
        Metadata = new Dictionary<string, object>
        {
            { "ProjectId", projectId },
            { "KeycloakId", keycloakId }
        };
    }
}
