using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Infrastructure.Persistence.Configurations;

public sealed class RepartitionSourceRevenuConfiguration : IEntityTypeConfiguration<RepartitionSourceRevenu>
{
    public void Configure(EntityTypeBuilder<RepartitionSourceRevenu> builder)
    {
        builder.ToTable("RepartitionsSourceRevenu");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.TransactionId).IsRequired();
        builder.Property(r => r.SourceRevenuId).IsRequired();

        builder.Property(r => r.Montant)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasIndex(r => r.TransactionId);
        builder.HasIndex(r => r.SourceRevenuId);

        builder.HasOne<SourceRevenu>()
            .WithMany()
            .HasForeignKey(r => r.SourceRevenuId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
