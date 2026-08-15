using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Infrastructure.Persistence.Configurations;

public sealed class SourceRevenuConfiguration : IEntityTypeConfiguration<SourceRevenu>
{
    public void Configure(EntityTypeBuilder<SourceRevenu> builder)
    {
        builder.ToTable("SourcesRevenu");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Nom)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(s => s.EstActif).IsRequired();

        builder.Property(s => s.UtilisateurId).IsRequired();
        builder.Property(s => s.DateCreation).IsRequired();

        builder.HasIndex(s => s.UtilisateurId);

        builder.HasOne<Utilisateur>()
            .WithMany()
            .HasForeignKey(s => s.UtilisateurId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
