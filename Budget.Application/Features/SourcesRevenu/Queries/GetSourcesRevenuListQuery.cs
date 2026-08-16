using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Common;
using Budget.Application.Dtos.SourcesRevenu;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.SourcesRevenu.Queries;

public sealed record GetSourcesRevenuListQuery(int Page, int PageSize) : IQuery<PagedResultDto<SourceRevenuDto>>;

public sealed class GetSourcesRevenuListQueryHandler(
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetSourcesRevenuListQuery, PagedResultDto<SourceRevenuDto>>
{
    public async ValueTask<PagedResultDto<SourceRevenuDto>> Handle(GetSourcesRevenuListQuery query, CancellationToken cancellationToken)
    {
        var utilisateurId = currentUserService.UtilisateurId
            ?? throw new ForbiddenAccessException("Utilisateur non authentifié.");

        var sourcesRevenu = await sourceRevenuRepository.GetByUtilisateurIdAsync(utilisateurId, query.Page, query.PageSize, cancellationToken);

        return new PagedResultDto<SourceRevenuDto>(
            sourcesRevenu.Items.Select(s => s.ToDto()).ToList(),
            sourcesRevenu.TotalCount,
            sourcesRevenu.Page,
            sourcesRevenu.PageSize);
    }
}
