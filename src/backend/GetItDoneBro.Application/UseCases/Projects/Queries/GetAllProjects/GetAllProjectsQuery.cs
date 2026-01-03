using GetItDoneBro.Application.Common.Interfaces.Messaging;
using GetItDoneBro.Application.UseCases.Projects.Queries.GetProjectById;

namespace GetItDoneBro.Application.UseCases.Projects.Queries.GetAllProjects;

public record GetAllProjectsQuery : IQuery<List<ProjectDto>>;
