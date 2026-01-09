using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.Tasks.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GetItDoneBro.Application.UseCases.Tasks.Queries.GetTasksByProject;

public interface IGetTasksByProjectHandler
{
    Task<IEnumerable<TaskDto>> HandleAsync(Guid projectId, CancellationToken cancellationToken);
}

public sealed class GetTasksByProjectHandler(
    IRepository repository)
    : IGetTasksByProjectHandler
{
    public async Task<IEnumerable<TaskDto>> HandleAsync(
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var tasks = await repository.ProjectTasks
            .Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return tasks.Select(t => new TaskDto(
            t.Id,
            t.ProjectId,
            t.TaskColumnId,
            t.Title,
            t.Description,
            t.AssignedToKeycloakId,
            t.ImageUrl,
            t.CreatedAtUtc,
            t.UpdatedAtUtc));
    }
}
