using GetItDoneBro.Application.Common.Interfaces.Messaging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(Guid Id) : ICommand;
