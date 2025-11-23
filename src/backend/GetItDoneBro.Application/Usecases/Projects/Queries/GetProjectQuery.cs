using GetItDoneBro.Application.Common.Interfaces.Messaging;
using GetItDoneBro.Application.Usecases.Projects.DtoS;

namespace GetItDoneBro.Application.Usecases.Projects.Queries;

public class GetProjectQuery : IQuery<ProjectDto>
{
    public int Id { get; set; }
}
