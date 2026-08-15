using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.SourcesRevenu;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.SourcesRevenu.Queries;

public sealed record GetSourceRevenuByIdQuery(Guid Id) : IQuery<SourceRevenuDto>;

public sealed class GetSourceRevenuByIdQueryHandler(
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetSourceRevenuByIdQuery, SourceRevenuDto>
{
    public async ValueTask<SourceRevenuDto> Handle(GetSourceRevenuByIdQuery query, CancellationToken cancellationToken)
    {
        var sourceRevenu = await sourceRevenuRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(SourceRevenu), query.Id);

        SourceRevenuAuthorizationGuard.EnsureOwnership(sourceRevenu, currentUserService);

        return sourceRevenu.ToDto();
    }
}
