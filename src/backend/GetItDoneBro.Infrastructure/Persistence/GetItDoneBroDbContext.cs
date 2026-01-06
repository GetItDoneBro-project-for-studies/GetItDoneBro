using GetItDoneBro.Domain.Interfaces;
using GetItDoneBro.Domain.Projects;
using Microsoft.EntityFrameworkCore;
namespace GetItDoneBro.Infrastructure.Persistence;

public class GetItDoneBroDbContext(DbContextOptions<GetItDoneBroDbContext> options) : DbContext(options), IRepository
{
    public DbSet<Project> Projects => Set<Project>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("unaccent");

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GetItDoneBroDbContext).Assembly);
    }

}
