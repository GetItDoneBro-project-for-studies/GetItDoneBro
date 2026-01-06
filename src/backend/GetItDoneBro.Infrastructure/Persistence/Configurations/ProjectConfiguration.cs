using GetItDoneBro.Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetItDoneBro.Infrastructure.Persistence.Configurations;

internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(4000);

        builder.HasIndex(p => p.Name).IsUnique();

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("now() at time zone 'utc'");

        builder.Property(p => p.UpdatedAt);
    }
}
