using GetItDoneBro.Application.Common.Interfaces.Messaging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(Guid Id, string Name, string Description) : ICommand<UpdateProjectResponse>;

public record UpdateProjectResponse(Guid Id, string Name);
