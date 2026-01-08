namespace GetItDoneBro.Application.Common.Interfaces.Services;

public interface IProjectsRepository
{
    Task<bool> IsNameExistsAsync(string name, CancellationToken cancellationToken = default);
}
