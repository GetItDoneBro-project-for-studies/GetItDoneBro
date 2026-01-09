namespace GetItDoneBro.Application.Exceptions;

public class UserNotAssignedException : Exception
{
    public string Code { get; } = "USER_NOT_ASSIGNED";
    public Guid ProjectId { get; }
    public string KeycloakId { get; }
    public Dictionary<string, object> Metadata { get; }

    public UserNotAssignedException(Guid projectId, string keycloakId)
        : base($"Uzytkownik '{keycloakId}' nie jest przypisany do projektu '{projectId}'.")
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