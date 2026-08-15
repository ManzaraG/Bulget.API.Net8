using Budget.Application.Contrats.Repositories;
using Budget.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Infrastructure.Persistence;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<BudgetDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<ISourceRevenuRepository, SourceRevenuRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ICategorieRepository, CategorieRepository>();
        services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();

        return services;
    }
}
