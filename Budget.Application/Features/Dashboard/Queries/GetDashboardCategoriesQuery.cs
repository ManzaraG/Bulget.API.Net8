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

        var depenses = transactions.Where(t => t.EstDepense).ToList();
        var total = depenses.Sum(t => t.Montant);

        var nomParCategorie = categories.ToDictionary(c => c.Id, c => c.Nom);

        // Les dépenses sans catégorie (ou dont la catégorie a disparu) sont regroupées sous Guid.Empty.
        return depenses
            .GroupBy(t => t.CategorieId.HasValue && nomParCategorie.ContainsKey(t.CategorieId.Value) ? t.CategorieId.Value : Guid.Empty)
            .Select(g =>
            {
                var montant = g.Sum(t => t.Montant);
                var nom = g.Key == Guid.Empty ? "Sans catégorie" : nomParCategorie[g.Key];
                var pourcentage = total > 0 ? Math.Round(montant / total * 100, 2) : 0m;
                return new CategorieBudgetDto(g.Key, nom, montant, pourcentage);
            })
            .OrderByDescending(c => c.Montant)
            .ToList();
    }
}
