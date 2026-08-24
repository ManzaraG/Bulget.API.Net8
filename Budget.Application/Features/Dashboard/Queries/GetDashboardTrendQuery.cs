using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Dashboard;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Dashboard.Queries;

public sealed record GetDashboardTrendQuery(int Annee, int Mois, int NombreMois = 6) : IQuery<IReadOnlyList<DashboardTrendPointDto>>;

public sealed class GetDashboardTrendQueryHandler(
    ITransactionRepository transactionRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetDashboardTrendQuery, IReadOnlyList<DashboardTrendPointDto>>
{
    public async ValueTask<IReadOnlyList<DashboardTrendPointDto>> Handle(GetDashboardTrendQuery query, CancellationToken cancellationToken)
    {
        var utilisateurId = currentUserService.UtilisateurId
            ?? throw new ForbiddenAccessException("Utilisateur non authentifié.");

        var nombreMois = Math.Max(1, query.NombreMois);
        var moisCourant = new DateTime(query.Annee, query.Mois, 1, 0, 0, 0, DateTimeKind.Utc);
        var debut = moisCourant.AddMonths(-(nombreMois - 1));
        var fin = moisCourant.AddMonths(1);

        var transactions = await transactionRepository.GetByUtilisateurIdAndPeriodAsync(
            utilisateurId, debut, fin, cancellationToken);

        var totauxParMois = transactions
            .GroupBy(t => new { t.Date.Year, t.Date.Month })
            .ToDictionary(
                g => (g.Key.Year, g.Key.Month),
                g => (
                    Revenus: g.Where(t => t.EstRevenu).Sum(t => t.Montant),
                    Depenses: g.Where(t => t.EstDepense).Sum(t => t.Montant)));

        var points = new List<DashboardTrendPointDto>();
        for (var i = 0; i < nombreMois; i++)
        {
            var mois = debut.AddMonths(i);
            totauxParMois.TryGetValue((mois.Year, mois.Month), out var totaux);
            points.Add(new DashboardTrendPointDto(mois.Year, mois.Month, totaux.Revenus, totaux.Depenses));
        }

        return points;
    }
}
