using GetItDoneBro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace GetItDoneBro.Infrastructure.Persistence;

public class GetItDoneBroDbContext(DbContextOptions<GetItDoneBroDbContext> options) : DbContext(options), IRepository
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("unaccent");

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GetItDoneBroDbContext).Assembly);
    }

}
