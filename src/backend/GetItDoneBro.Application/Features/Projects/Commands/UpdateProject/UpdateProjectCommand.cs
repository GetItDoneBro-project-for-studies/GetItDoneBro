using GetItDoneBro.Application.Common.Interfaces.Messaging;

namespace GetItDoneBro.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(Guid Id, string Name, string Description) : ICommand<UpdateProjectResponse>;

public record UpdateProjectResponse(Guid Id, string Name);
