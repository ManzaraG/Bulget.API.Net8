using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Infrastructure.Persistence.Configurations;

public sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Montant)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(t => t.Date).IsRequired();

        builder.Property(t => t.Description).HasMaxLength(500);

        builder.Property(t => t.CompteId).IsRequired();

        builder.HasIndex(t => t.CompteId);
        builder.HasIndex(t => new { t.CompteId, t.Date });

        builder.HasOne<Compte>()
            .WithMany()
            .HasForeignKey(t => t.CompteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Categorie>()
            .WithMany()
            .HasForeignKey(t => t.CategorieId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
