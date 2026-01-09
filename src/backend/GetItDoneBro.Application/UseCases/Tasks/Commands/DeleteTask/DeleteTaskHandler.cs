using GetItDoneBro.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Tasks.Commands.DeleteTask;

public interface IDeleteTaskHandler
{
    Task HandleAsync(DeleteTaskCommand command, CancellationToken cancellationToken);
}

public sealed class DeleteTaskHandler(
    IRepository repository,
    ILogger<DeleteTaskHandler> logger)
    : IDeleteTaskHandler
{
    public async Task HandleAsync(
        DeleteTaskCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Deleting task {TaskId}",
            command.Id);

        var task = await repository.ProjectTasks
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (task == null)
        {
            logger.LogWarning("Task {TaskId} not found", command.Id);
            throw new InvalidOperationException($"Task with ID {command.Id} not found");
        }

        repository.ProjectTasks.Remove(task);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Deleted task {TaskId}",
            command.Id);
    }
}
