using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;

namespace Budget.Application.Features.Comptes;

internal static class CompteAuthorizationGuard
{
    public static void EnsureOwnership(Compte compte, ICurrentUserService currentUserService)
    {
        if (compte.UtilisateurId != currentUserService.UtilisateurId)
            throw new ForbiddenAccessException("Ce compte n'appartient pas à l'utilisateur courant.");
    }

    public static async ValueTask<Compte> EnsureCompteOwnershipAsync(
        Guid compteId,
        ICompteRepository compteRepository,
        ICurrentUserService currentUserService,
        CancellationToken cancellationToken)
    {
        var compte = await compteRepository.GetByIdAsync(compteId, cancellationToken)
            ?? throw new NotFoundException(nameof(Compte), compteId);

        EnsureOwnership(compte, currentUserService);

        return compte;
    }
}
