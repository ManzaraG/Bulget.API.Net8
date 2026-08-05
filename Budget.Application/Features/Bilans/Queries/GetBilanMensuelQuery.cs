using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Bilans;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Bilans.Queries;

public sealed record GetBilanMensuelQuery(int Annee, int Mois) : IQuery<BilanMensuelDto>;

public sealed class GetBilanMensuelQueryHandler(
    ITransactionRepository transactionRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetBilanMensuelQuery, BilanMensuelDto>
{
    public async ValueTask<BilanMensuelDto> Handle(GetBilanMensuelQuery query, CancellationToken cancellationToken)
    {
        var utilisateurId = currentUserService.UtilisateurId
            ?? throw new ForbiddenAccessException("Utilisateur non authentifié.");

        var debut = new DateTime(query.Annee, query.Mois, 1, 0, 0, 0, DateTimeKind.Utc);
        var fin = debut.AddMonths(1);

        var transactions = await transactionRepository.GetByUtilisateurIdAndPeriodAsync(
            utilisateurId, debut, fin, cancellationToken);

        var totalRevenus = transactions.Where(t => t.EstRevenu).Sum(t => t.Montant);
        var totalDepenses = transactions.Where(t => t.EstDepense).Sum(t => t.Montant);

        return new BilanMensuelDto(utilisateurId, query.Annee, query.Mois, totalRevenus, totalDepenses, totalRevenus - totalDepenses);
    }
}
