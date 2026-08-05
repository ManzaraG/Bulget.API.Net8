using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Infrastructure.Persistence.Configurations;

public sealed class CompteConfiguration : IEntityTypeConfiguration<Compte>
{
    public void Configure(EntityTypeBuilder<Compte> builder)
    {
        builder.ToTable("Comptes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nom)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.UtilisateurId).IsRequired();
        builder.Property(c => c.DateCreation).IsRequired();

        builder.HasIndex(c => c.UtilisateurId);

        builder.HasOne<Utilisateur>()
            .WithMany()
            .HasForeignKey(c => c.UtilisateurId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
