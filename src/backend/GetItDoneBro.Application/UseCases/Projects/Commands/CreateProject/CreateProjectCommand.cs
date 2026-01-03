using GetItDoneBro.Application.Common.Interfaces.Messaging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.CreateProject;

public record CreateProjectCommand(string Name, string Description) : ICommand<CreateProjectResponse>;

public record CreateProjectResponse(Guid Id, string Name);
