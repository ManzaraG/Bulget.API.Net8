using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Comptes;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Comptes.Queries;

public sealed record GetComptesListQuery : IQuery<IReadOnlyList<CompteDto>>;

public sealed class GetComptesListQueryHandler(
    ICompteRepository compteRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetComptesListQuery, IReadOnlyList<CompteDto>>
{
    public async ValueTask<IReadOnlyList<CompteDto>> Handle(GetComptesListQuery query, CancellationToken cancellationToken)
    {
        var utilisateurId = currentUserService.UtilisateurId
            ?? throw new ForbiddenAccessException("Utilisateur non authentifié.");

        var comptes = await compteRepository.GetByUtilisateurIdAsync(utilisateurId, cancellationToken);

        return comptes.Select(c => c.ToDto()).ToList();
    }
}
