namespace GetItDoneBro.Application.Exceptions;

public class DuplicateProjectException : Exception
{
    public string Code { get; } = "DUPLICATE_PROJECT";
    public string ProjectName { get; }
    public Guid? ExistingProjectId { get; }
    public Dictionary<string, object> Metadata { get; }

    public DuplicateProjectException(string projectName, Guid? existingProjectId = null)
        : base($"A project with the name '{projectName}' already exists.")
    {
        ProjectName = projectName;
        ExistingProjectId = existingProjectId;
        Metadata = new Dictionary<string, object>
        {
            { "ProjectName", projectName },
        };

        if (existingProjectId.HasValue)
        {
            Metadata["ExistingProjectId"] = existingProjectId.Value;
        }
           
    }
}
