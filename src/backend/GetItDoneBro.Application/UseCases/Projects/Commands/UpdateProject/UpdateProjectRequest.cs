namespace GetItDoneBro.Application.UseCases.Projects.Commands.UpdateProject;

public record UpdateProjectRequest(Guid Id, string Name, string Description);
