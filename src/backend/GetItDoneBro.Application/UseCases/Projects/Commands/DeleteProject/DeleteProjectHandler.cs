using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Application.Exceptions;
using GetItDoneBro.Domain.Entities;
using GetItDoneBro.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace GetItDoneBro.Application.UseCases.Projects.Commands.DeleteProject;

public interface IDeleteProjectHandler
{
    Task HandleAsync(DeleteProjectCommand command, CancellationToken cancellationToken);
}

public sealed class DeleteProjectHandler(
    IRepository repository,
    ILogger<DeleteProjectHandler> logger)
    : IDeleteProjectHandler
{
    public async Task HandleAsync(DeleteProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting project {ProjectId}", command.Id);

        Project project = await repository.Projects.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
                          ?? throw new NotFoundException(nameof(DeleteProjectHandler),
                              "Nie znaleziono projektu o podanym identyfikatorze!");

        EntityEntry<Project> deleted = repository.Projects.Remove(project);
        if (deleted.State != EntityState.Deleted)
        {
            logger.LogWarning("Nie znaleziono podanego projektu {ProjectId}", command.Id);
            throw new ProjectNotFoundException(command.Id);
        }

        await repository.SaveChangesAsync(cancellationToken);

        logger.LogDebug("Usunięto projekt {ProjectId}", command.Id);
    }
}
