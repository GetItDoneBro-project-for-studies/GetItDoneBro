using GetItDoneBro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetItDoneBro.Infrastructure.Persistence.Configurations;

internal sealed class ProjectUserConfiguration : IEntityTypeConfiguration<ProjectUser>
{
    public void Configure(EntityTypeBuilder<ProjectUser> builder)
    {
        builder.Property(pu => pu.UserId)
            .IsRequired();

        builder.Property(pu => pu.Role)
            .IsRequired();

        builder.HasIndex(pu => new { pu.ProjectId, pu.UserId })
            .IsUnique();

        builder.HasIndex(pu => pu.UserId);

        builder.HasOne(pu => pu.Project)
            .WithMany(p => p.ProjectUsers)
            .HasForeignKey(pu => pu.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}