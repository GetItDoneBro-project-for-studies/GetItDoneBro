using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.TaskColumns.Commands.CreateTaskColumn;

public interface ICreateTaskColumnHandler
{
    Task<Guid> HandleAsync(CreateTaskColumnCommand command, CancellationToken cancellationToken);
}

public sealed class CreateTaskColumnHandler(
    IRepository repository,
    ILogger<CreateTaskColumnHandler> logger)
    : ICreateTaskColumnHandler
{
    public async Task<Guid> HandleAsync(
        CreateTaskColumnCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating task column with name {ColumnName} for project {ProjectId}",
            command.Name,
            command.ProjectId);

        var taskColumn = TaskColumn.Create(command.ProjectId, command.Name, command.OrderIndex);

        await repository.TaskColumns.AddAsync(taskColumn, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Created task column with ID {TaskColumnId}",
            taskColumn.Id);

        return taskColumn.Id;
    }
}
