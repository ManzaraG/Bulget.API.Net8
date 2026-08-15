using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.SourcesRevenu;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.SourcesRevenu.Queries;

public sealed record GetSourcesRevenuListQuery : IQuery<IReadOnlyList<SourceRevenuDto>>;

public sealed class GetSourcesRevenuListQueryHandler(
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetSourcesRevenuListQuery, IReadOnlyList<SourceRevenuDto>>
{
    public async ValueTask<IReadOnlyList<SourceRevenuDto>> Handle(GetSourcesRevenuListQuery query, CancellationToken cancellationToken)
    {
        var utilisateurId = currentUserService.UtilisateurId
            ?? throw new ForbiddenAccessException("Utilisateur non authentifié.");

        var sourcesRevenu = await sourceRevenuRepository.GetByUtilisateurIdAsync(utilisateurId, cancellationToken);

        return sourcesRevenu.Select(s => s.ToDto()).ToList();
    }
}
