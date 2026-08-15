using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;

namespace Budget.Application.Features.SourcesRevenu;

internal static class SourceRevenuAuthorizationGuard
{
    public static void EnsureOwnership(SourceRevenu sourceRevenu, ICurrentUserService currentUserService)
    {
        if (sourceRevenu.UtilisateurId != currentUserService.UtilisateurId)
            throw new ForbiddenAccessException("Cette source de revenu n'appartient pas à l'utilisateur courant.");
    }

    public static async ValueTask<SourceRevenu> EnsureSourceRevenuOwnershipAsync(
        Guid sourceRevenuId,
        ISourceRevenuRepository sourceRevenuRepository,
        ICurrentUserService currentUserService,
        CancellationToken cancellationToken)
    {
        var sourceRevenu = await sourceRevenuRepository.GetByIdAsync(sourceRevenuId, cancellationToken)
            ?? throw new NotFoundException(nameof(SourceRevenu), sourceRevenuId);

        EnsureOwnership(sourceRevenu, currentUserService);

        return sourceRevenu;
    }
}
