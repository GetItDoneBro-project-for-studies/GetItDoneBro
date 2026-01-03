namespace GetItDoneBro.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectRequest(Guid Id, string Name, string Description);
