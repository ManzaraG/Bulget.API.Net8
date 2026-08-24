using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Dashboard;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Dashboard.Queries;

public sealed record GetDashboardCategoriesQuery(int Annee, int Mois) : IQuery<IReadOnlyList<CategorieBudgetDto>>;

public sealed class GetDashboardCategoriesQueryHandler(
    ITransactionRepository transactionRepository,
    ICategorieRepository categorieRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetDashboardCategoriesQuery, IReadOnlyList<CategorieBudgetDto>>
{
    public async ValueTask<IReadOnlyList<CategorieBudgetDto>> Handle(GetDashboardCategoriesQuery query, CancellationToken cancellationToken)
    {
        var utilisateurId = currentUserService.UtilisateurId
            ?? throw new ForbiddenAccessException("Utilisateur non authentifié.");

        var debut = new DateTime(query.Annee, query.Mois, 1, 0, 0, 0, DateTimeKind.Utc);
        var fin = debut.AddMonths(1);

        var transactions = await transactionRepository.GetByUtilisateurIdAndPeriodAsync(
            utilisateurId, debut, fin, cancellationToken);
        var categories = await categorieRepository.GetAllAsync(cancellationToken);

        var montantParCategorie = transactions
            .Where(t => t.CategorieId.HasValue)
            .GroupBy(t => t.CategorieId!.Value)
            .ToDictionary(g => g.Key, g => g.Sum(t => t.Montant));

        var total = montantParCategorie.Values.Sum();

        return categories
            .Select(c =>
            {
                var montant = montantParCategorie.GetValueOrDefault(c.Id);
                var pourcentage = total > 0 ? Math.Round(montant / total * 100, 2) : 0m;
                return new CategorieBudgetDto(c.Id, c.Nom, montant, pourcentage);
            })
            .OrderByDescending(c => c.Montant)
            .ToList();
    }
}
