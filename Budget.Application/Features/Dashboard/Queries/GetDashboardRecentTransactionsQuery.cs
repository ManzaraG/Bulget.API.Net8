using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Dashboard;
using Budget.Application.Dtos.Transactions;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Dashboard.Queries;

public sealed record GetDashboardRecentTransactionsQuery(int Annee, int Mois, int Limite = 5) : IQuery<IReadOnlyList<TransactionRecenteDto>>;

public sealed class GetDashboardRecentTransactionsQueryHandler(
    ITransactionRepository transactionRepository,
    ISourceRevenuRepository sourceRevenuRepository,
    ICategorieRepository categorieRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetDashboardRecentTransactionsQuery, IReadOnlyList<TransactionRecenteDto>>
{
    public async ValueTask<IReadOnlyList<TransactionRecenteDto>> Handle(GetDashboardRecentTransactionsQuery query, CancellationToken cancellationToken)
    {
        var utilisateurId = currentUserService.UtilisateurId
            ?? throw new ForbiddenAccessException("Utilisateur non authentifié.");

        var debut = new DateTime(query.Annee, query.Mois, 1, 0, 0, 0, DateTimeKind.Utc);
        var fin = debut.AddMonths(1);

        var transactions = await transactionRepository.GetByUtilisateurIdAndPeriodAsync(
            utilisateurId, debut, fin, cancellationToken);
        var sourcesRevenu = await sourceRevenuRepository.GetAllByUtilisateurIdAsync(utilisateurId, cancellationToken);
        var nomParSource = sourcesRevenu.ToDictionary(s => s.Id, s => s.Nom);
        var nomParCategorie = (await categorieRepository.GetAllAsync(cancellationToken)).ToDictionary(c => c.Id, c => c.Nom);

        return transactions
            .OrderByDescending(t => t.Date)
            .Take(Math.Clamp(query.Limite, 1, 50))
            .Select(t => new TransactionRecenteDto(
                t.Id,
                t.Montant,
                t.EstRevenu ? TypeTransactionDto.Revenu : TypeTransactionDto.Depense,
                t.Date,
                t.Description,
                t.CategorieId,
                t.CategorieId.HasValue ? nomParCategorie.GetValueOrDefault(t.CategorieId.Value, "Sans catégorie") : "Sans catégorie",
                t.Repartitions
                    .Select(r => nomParSource.GetValueOrDefault(r.SourceRevenuId, string.Empty))
                    .Where(nom => nom.Length > 0)
                    .ToList()))
            .ToList();
    }
}
