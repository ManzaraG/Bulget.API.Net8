using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Dashboard;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Dashboard.Queries;

public sealed record GetDashboardComptesQuery(int Annee, int Mois) : IQuery<IReadOnlyList<SourceRevenuBudgetDto>>;

public sealed class GetDashboardComptesQueryHandler(
    ITransactionRepository transactionRepository,
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetDashboardComptesQuery, IReadOnlyList<SourceRevenuBudgetDto>>
{
    public async ValueTask<IReadOnlyList<SourceRevenuBudgetDto>> Handle(GetDashboardComptesQuery query, CancellationToken cancellationToken)
    {
        var utilisateurId = currentUserService.UtilisateurId
            ?? throw new ForbiddenAccessException("Utilisateur non authentifié.");

        var debut = new DateTime(query.Annee, query.Mois, 1, 0, 0, 0, DateTimeKind.Utc);
        var fin = debut.AddMonths(1);

        var transactions = await transactionRepository.GetByUtilisateurIdAndPeriodAsync(
            utilisateurId, debut, fin, cancellationToken);
        var sourcesRevenu = await sourceRevenuRepository.GetAllByUtilisateurIdAsync(utilisateurId, cancellationToken);

        var montantParSource = transactions
            .Where(t => t.EstRevenu)
            .SelectMany(t => t.Repartitions)
            .GroupBy(r => r.SourceRevenuId)
            .ToDictionary(g => g.Key, g => g.Sum(r => r.Montant));

        var totalRevenus = montantParSource.Values.Sum();

        return sourcesRevenu
            .Select(s =>
            {
                var montant = montantParSource.GetValueOrDefault(s.Id);
                var pourcentage = totalRevenus > 0 ? Math.Round(montant / totalRevenus * 100, 2) : 0m;
                return new SourceRevenuBudgetDto(s.Id, s.Nom, montant, pourcentage);
            })
            .OrderByDescending(s => s.Montant)
            .ToList();
    }
}
