using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Infrastructure.Persistence.Configurations;

public sealed class CategorieConfiguration : IEntityTypeConfiguration<Categorie>
{
    public void Configure(EntityTypeBuilder<Categorie> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nom)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}
