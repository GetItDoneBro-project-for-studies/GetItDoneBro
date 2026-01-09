using GetItDoneBro.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.TaskColumns.Commands.DeleteTaskColumn;

public interface IDeleteTaskColumnHandler
{
    Task HandleAsync(DeleteTaskColumnCommand command, CancellationToken cancellationToken);
}

public sealed class DeleteTaskColumnHandler(
    IRepository repository,
    ILogger<DeleteTaskColumnHandler> logger)
    : IDeleteTaskColumnHandler
{
    public async Task HandleAsync(
        DeleteTaskColumnCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Deleting task column {TaskColumnId}",
            command.Id);

        var taskColumn = await repository.TaskColumns
            .FirstOrDefaultAsync(tc => tc.Id == command.Id, cancellationToken);

        if (taskColumn == null)
        {
            logger.LogWarning("Task column {TaskColumnId} not found", command.Id);
            throw new InvalidOperationException($"Task column with ID {command.Id} not found");
        }

        repository.TaskColumns.Remove(taskColumn);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Deleted task column {TaskColumnId}",
            command.Id);
    }
}
