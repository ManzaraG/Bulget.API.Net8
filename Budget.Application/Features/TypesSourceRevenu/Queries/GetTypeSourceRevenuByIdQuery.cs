using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.TypesSourceRevenu;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.TypesSourceRevenu.Queries;

public sealed record GetTypeSourceRevenuByIdQuery(Guid Id) : IQuery<TypeSourceRevenuDto>;

public sealed class GetTypeSourceRevenuByIdQueryHandler(ITypeSourceRevenuRepository typeSourceRevenuRepository)
    : IQueryHandler<GetTypeSourceRevenuByIdQuery, TypeSourceRevenuDto>
{
    public async ValueTask<TypeSourceRevenuDto> Handle(GetTypeSourceRevenuByIdQuery query, CancellationToken cancellationToken)
    {
        var typeSourceRevenu = await typeSourceRevenuRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(TypeSourceRevenu), query.Id);

        return typeSourceRevenu.ToDto();
    }
}
