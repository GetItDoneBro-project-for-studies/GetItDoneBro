using GetItDoneBro.Application.Common.Interfaces.Messaging;

namespace GetItDoneBro.Application.Features.Projects.Queries.GetProjectById;

public record GetProjectByIdQuery(Guid Id) : IQuery<ProjectDto?>;

public record ProjectDto(Guid Id, string Name, string Description);
