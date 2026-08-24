using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Infrastructure.Persistence.Configurations;

public sealed class DeviseConfiguration : IEntityTypeConfiguration<Devise>
{
    public void Configure(EntityTypeBuilder<Devise> builder)
    {
        builder.ToTable("Devises");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Nom)
            .IsRequired()
            .HasMaxLength(200);
    }
}
