using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Budget.Infrastructure.Persistence;

/// <summary>
/// Utilisée uniquement par l'outillage design-time d'EF Core (dotnet ef migrations ...).
/// L'application elle-même construit le DbContext via <see cref="PersistenceServiceCollectionExtensions.AddPersistence"/>.
/// </summary>
public sealed class BudgetDbContextFactory : IDesignTimeDbContextFactory<BudgetDbContext>
{
    public BudgetDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BudgetDbContext>();
        optionsBuilder.UseSqlServer("Server=INEXA-DEV-GBANE\\MSSQLSERVER01;Database=BudgetDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new BudgetDbContext(optionsBuilder.Options);
    }
}
