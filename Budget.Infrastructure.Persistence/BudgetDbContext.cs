using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Persistence;

public sealed class BudgetDbContext(DbContextOptions<BudgetDbContext> options) : DbContext(options)
{
    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
    public DbSet<Compte> Comptes => Set<Compte>();
    public DbSet<Categorie> Categories => Set<Categorie>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
