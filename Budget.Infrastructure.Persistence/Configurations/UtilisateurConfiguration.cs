using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Infrastructure.Persistence.Configurations;

public sealed class UtilisateurConfiguration : IEntityTypeConfiguration<Utilisateur>
{
    public void Configure(EntityTypeBuilder<Utilisateur> builder)
    {
        builder.ToTable("Utilisateurs");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nom)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(320);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.MotDePasseHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.DateCreation).IsRequired();
    }
}
