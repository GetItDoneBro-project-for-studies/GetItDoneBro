using GetItDoneBro.Application.Common.Interfaces.Messaging;
using GetItDoneBro.Application.Usecases.Projects.DtoS;

namespace GetItDoneBro.Application.Usecases.Projects.Commands;

public class CreateProjectCommand : ICommand<ProjectDto>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
