using GetItDoneBro.Application.Common.Interfaces.Messaging;

namespace GetItDoneBro.Application.Features.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(Guid Id) : ICommand;
