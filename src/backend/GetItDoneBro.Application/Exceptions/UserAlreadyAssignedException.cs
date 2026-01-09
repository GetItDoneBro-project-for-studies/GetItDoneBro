namespace GetItDoneBro.Application.Exceptions;

public class UserAlreadyAssignedException : Exception
{
    public string Code { get; } = "USER_ALREADY_ASSIGNED";
    public Guid ProjectId { get; }
    public string KeycloakId { get; }
    public Dictionary<string, object> Metadata { get; }

    public UserAlreadyAssignedException(Guid projectId, string keycloakId)
        : base($"Uzytkownik '{keycloakId}' jest juz przypisany do projektu '{projectId}'.")
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