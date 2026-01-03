using GetItDoneBro.Application.Common.Interfaces.Messaging;
using GetItDoneBro.Application.Features.Projects.Queries.GetProjectById;

namespace GetItDoneBro.Application.Features.Projects.Queries.GetAllProjects;

public record GetAllProjectsQuery : IQuery<List<ProjectDto>>;
