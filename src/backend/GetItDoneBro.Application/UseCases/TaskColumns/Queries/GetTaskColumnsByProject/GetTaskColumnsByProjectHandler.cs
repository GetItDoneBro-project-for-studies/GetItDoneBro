using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.UseCases.TaskColumns.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GetItDoneBro.Application.UseCases.TaskColumns.Queries.GetTaskColumnsByProject;

public interface IGetTaskColumnsByProjectHandler
{
    Task<IEnumerable<TaskColumnDto>> HandleAsync(Guid projectId, CancellationToken cancellationToken);
}

public sealed class GetTaskColumnsByProjectHandler(
    IRepository repository)
    : IGetTaskColumnsByProjectHandler
{
    public async Task<IEnumerable<TaskColumnDto>> HandleAsync(
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var taskColumns = await repository.TaskColumns
            .Where(tc => tc.ProjectId == projectId)
            .OrderBy(tc => tc.OrderIndex)
            .ToListAsync(cancellationToken);

        return taskColumns.Select(tc => new TaskColumnDto(
            tc.Id,
            tc.ProjectId,
            tc.Name,
            tc.OrderIndex,
            tc.CreatedAtUtc,
            tc.UpdatedAtUtc));
    }
}
