using GetItDoneBro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetItDoneBro.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name)
            .IsRequired();
        builder.Property(p => p.Description)
            .HasMaxLength(300);
        builder.Property(p => p.CreatedAt)
            .HasColumnType("timestamp with time zone");
    }
}
