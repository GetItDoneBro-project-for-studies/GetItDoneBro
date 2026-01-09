using GetItDoneBro.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.TaskColumns.Commands.UpdateTaskColumn;

public interface IUpdateTaskColumnHandler
{
    Task HandleAsync(UpdateTaskColumnCommand command, CancellationToken cancellationToken);
}

public sealed class UpdateTaskColumnHandler(
    IRepository repository,
    ILogger<UpdateTaskColumnHandler> logger)
    : IUpdateTaskColumnHandler
{
    public async Task HandleAsync(
        UpdateTaskColumnCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Updating task column {TaskColumnId}",
            command.Id);

        var taskColumn = await repository.TaskColumns
            .FirstOrDefaultAsync(tc => tc.Id == command.Id, cancellationToken);

        if (taskColumn == null)
        {
            logger.LogWarning("Task column {TaskColumnId} not found", command.Id);
            throw new InvalidOperationException($"Task column with ID {command.Id} not found");
        }

        taskColumn.SetName(command.Name);
        taskColumn.SetOrderIndex(command.OrderIndex);

        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Updated task column {TaskColumnId}",
            taskColumn.Id);
    }
}
