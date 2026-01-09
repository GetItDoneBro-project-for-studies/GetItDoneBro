using GetItDoneBro.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Tasks.Commands.MoveTaskToColumn;

public interface IMoveTaskToColumnHandler
{
    Task HandleAsync(MoveTaskToColumnCommand command, CancellationToken cancellationToken);
}

public sealed class MoveTaskToColumnHandler(
    IRepository repository,
    ILogger<MoveTaskToColumnHandler> logger)
    : IMoveTaskToColumnHandler
{
    public async Task HandleAsync(
        MoveTaskToColumnCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Moving task {TaskId} to column {TaskColumnId}",
            command.TaskId,
            command.TaskColumnId);

        var task = await repository.ProjectTasks
            .FirstOrDefaultAsync(t => t.Id == command.TaskId, cancellationToken);

        if (task == null)
        {
            logger.LogWarning("Task {TaskId} not found", command.TaskId);
            throw new InvalidOperationException($"Task with ID {command.TaskId} not found");
        }

        task.MoveToColumn(command.TaskColumnId);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Moved task {TaskId} to column {TaskColumnId}",
            command.TaskId,
            command.TaskColumnId);
    }
}
