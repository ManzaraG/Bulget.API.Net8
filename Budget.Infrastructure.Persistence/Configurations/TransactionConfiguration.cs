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

        builder.Property(t => t.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(t => t.Date).IsRequired();

        builder.Property(t => t.Description).HasMaxLength(500);

        builder.HasIndex(t => t.Date);

        builder.HasOne<Categorie>()
            .WithMany()
            .HasForeignKey(t => t.CategorieId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(t => t.Repartitions)
            .WithOne()
            .HasForeignKey(r => r.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(t => t.Repartitions).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
