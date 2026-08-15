using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Transactions;
using Budget.Application.Features.SourcesRevenu;
using Mediator;

namespace Budget.Application.Features.Transactions.Queries;

public sealed record GetTransactionsListQuery(Guid SourceRevenuId) : IQuery<IReadOnlyList<TransactionDto>>;

public sealed class GetTransactionsListQueryHandler(
    ITransactionRepository transactionRepository,
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetTransactionsListQuery, IReadOnlyList<TransactionDto>>
{
    public async ValueTask<IReadOnlyList<TransactionDto>> Handle(GetTransactionsListQuery query, CancellationToken cancellationToken)
    {
        await SourceRevenuAuthorizationGuard.EnsureSourceRevenuOwnershipAsync(
            query.SourceRevenuId, sourceRevenuRepository, currentUserService, cancellationToken);

        var transactions = await transactionRepository.GetBySourceRevenuIdAsync(query.SourceRevenuId, cancellationToken);

        return transactions.Select(t => t.ToDto()).ToList();
    }
}
