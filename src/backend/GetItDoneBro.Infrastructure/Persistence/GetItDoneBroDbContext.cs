using GetItDoneBro.Application.Common.Interfaces;
using GetItDoneBro.Domain.Abstract;
using GetItDoneBro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetItDoneBro.Infrastructure.Persistence;

public class GetItDoneBroDbContext(DbContextOptions<GetItDoneBroDbContext> options) : DbContext(options), IRepository
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectUser> ProjectUsers => Set<ProjectUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("unaccent");

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GetItDoneBroDbContext).Assembly);
        ConfigureEntityIdGeneration(modelBuilder);
    }

    private static void ConfigureEntityIdGeneration(ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entityType in modelBuilder
                     .Model.GetEntityTypes()
                     .Where(t => typeof(Entity).IsAssignableFrom(t.ClrType)))
        {
            EntityTypeBuilder builder = modelBuilder.Entity(entityType.ClrType);
            builder.Property(nameof(Entity.Id)).ValueGeneratedNever();
        }
    }
}