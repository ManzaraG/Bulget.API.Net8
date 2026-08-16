using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Infrastructure.Persistence.Configurations;

public sealed class TypeSourceRevenuConfiguration : IEntityTypeConfiguration<TypeSourceRevenu>
{
    public void Configure(EntityTypeBuilder<TypeSourceRevenu> builder)
    {
        builder.ToTable("TypesSourceRevenu");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Nom)
            .IsRequired()
            .HasMaxLength(200);
    }
}
