using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Transactions;
using Budget.Application.Features.Comptes;
using Mediator;

namespace Budget.Application.Features.Transactions.Queries;

public sealed record GetTransactionsListQuery(Guid CompteId) : IQuery<IReadOnlyList<TransactionDto>>;

public sealed class GetTransactionsListQueryHandler(
    ITransactionRepository transactionRepository,
    ICompteRepository compteRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetTransactionsListQuery, IReadOnlyList<TransactionDto>>
{
    public async ValueTask<IReadOnlyList<TransactionDto>> Handle(GetTransactionsListQuery query, CancellationToken cancellationToken)
    {
        await CompteAuthorizationGuard.EnsureCompteOwnershipAsync(
            query.CompteId, compteRepository, currentUserService, cancellationToken);

        var transactions = await transactionRepository.GetByCompteIdAsync(query.CompteId, cancellationToken);

        return transactions.Select(t => t.ToDto()).ToList();
    }
}
