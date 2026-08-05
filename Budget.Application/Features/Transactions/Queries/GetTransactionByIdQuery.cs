using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Transactions;
using Budget.Application.Features.Comptes;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Transactions.Queries;

public sealed record GetTransactionByIdQuery(Guid Id) : IQuery<TransactionDto>;

public sealed class GetTransactionByIdQueryHandler(
    ITransactionRepository transactionRepository,
    ICompteRepository compteRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetTransactionByIdQuery, TransactionDto>
{
    public async ValueTask<TransactionDto> Handle(GetTransactionByIdQuery query, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Transaction), query.Id);

        await CompteAuthorizationGuard.EnsureCompteOwnershipAsync(
            transaction.CompteId, compteRepository, currentUserService, cancellationToken);

        return transaction.ToDto();
    }
}
