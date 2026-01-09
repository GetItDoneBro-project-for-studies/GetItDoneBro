using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Tasks.Commands.CreateTask;

public interface ICreateTaskHandler
{
    Task<Guid> HandleAsync(CreateTaskCommand command, CancellationToken cancellationToken);
}

public sealed class CreateTaskHandler(
    IRepository repository,
    ILogger<CreateTaskHandler> logger)
    : ICreateTaskHandler
{
    public async Task<Guid> HandleAsync(
        CreateTaskCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating task with title {Title} for project {ProjectId}",
            command.Title,
            command.ProjectId);

        var task = ProjectTask.Create(
            command.ProjectId,
            command.TaskColumnId,
            command.Title,
            command.Description,
            command.AssignedToKeycloakId,
            command.ImageUrl);

        await repository.ProjectTasks.AddAsync(task, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Created task with ID {TaskId}",
            task.Id);

        return task.Id;
    }
}
