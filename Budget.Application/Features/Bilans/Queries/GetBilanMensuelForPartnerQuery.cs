using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Bilans;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Bilans.Queries;

public sealed record GetBilanMensuelForPartnerQuery(Guid UtilisateurId, int Annee, int Mois) : IQuery<BilanMensuelDto>;

public sealed class GetBilanMensuelForPartnerQueryHandler(
    ITransactionRepository transactionRepository,
    IUtilisateurRepository utilisateurRepository) : IQueryHandler<GetBilanMensuelForPartnerQuery, BilanMensuelDto>
{
    public async ValueTask<BilanMensuelDto> Handle(GetBilanMensuelForPartnerQuery query, CancellationToken cancellationToken)
    {
        _ = await utilisateurRepository.GetByIdAsync(query.UtilisateurId, cancellationToken)
            ?? throw new NotFoundException(nameof(Utilisateur), query.UtilisateurId);

        var debut = new DateTime(query.Annee, query.Mois, 1, 0, 0, 0, DateTimeKind.Utc);
        var fin = debut.AddMonths(1);

        var transactions = await transactionRepository.GetByUtilisateurIdAndPeriodAsync(
            query.UtilisateurId, debut, fin, cancellationToken);

        var totalRevenus = transactions.Where(t => t.EstRevenu).Sum(t => t.Montant);
        var totalDepenses = transactions.Where(t => t.EstDepense).Sum(t => t.Montant);

        return new BilanMensuelDto(query.UtilisateurId, query.Annee, query.Mois, totalRevenus, totalDepenses, totalRevenus - totalDepenses);
    }
}
