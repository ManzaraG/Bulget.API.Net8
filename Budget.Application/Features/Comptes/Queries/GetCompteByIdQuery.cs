using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Comptes;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Comptes.Queries;

public sealed record GetCompteByIdQuery(Guid Id) : IQuery<CompteDto>;

public sealed class GetCompteByIdQueryHandler(
    ICompteRepository compteRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetCompteByIdQuery, CompteDto>
{
    public async ValueTask<CompteDto> Handle(GetCompteByIdQuery query, CancellationToken cancellationToken)
    {
        var compte = await compteRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Compte), query.Id);

        CompteAuthorizationGuard.EnsureOwnership(compte, currentUserService);

        return compte.ToDto();
    }
}
