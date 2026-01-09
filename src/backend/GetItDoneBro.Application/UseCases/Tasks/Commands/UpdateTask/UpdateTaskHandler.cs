using GetItDoneBro.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Tasks.Commands.UpdateTask;

public interface IUpdateTaskHandler
{
    Task HandleAsync(UpdateTaskCommand command, CancellationToken cancellationToken);
}

public sealed class UpdateTaskHandler(
    IRepository repository,
    ILogger<UpdateTaskHandler> logger)
    : IUpdateTaskHandler
{
    public async Task HandleAsync(
        UpdateTaskCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Updating task {TaskId}",
            command.Id);

        var task = await repository.ProjectTasks
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (task == null)
        {
            logger.LogWarning("Task {TaskId} not found", command.Id);
            throw new InvalidOperationException($"Task with ID {command.Id} not found");
        }

        task.SetTitle(command.Title);
        task.SetDescription(command.Description);
        task.SetAssignedToKeycloakId(command.AssignedToKeycloakId);
        task.SetImageUrl(command.ImageUrl);

        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Updated task {TaskId}",
            task.Id);
    }
}
