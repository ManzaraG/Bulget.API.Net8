using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Common;
using Budget.Application.Dtos.Transactions;
using Budget.Application.Features.SourcesRevenu;
using Mediator;

namespace Budget.Application.Features.Transactions.Queries;

public sealed record GetTransactionsListQuery(Guid SourceRevenuId, int Page, int PageSize) : IQuery<PagedResultDto<TransactionDto>>;

public sealed class GetTransactionsListQueryHandler(
    ITransactionRepository transactionRepository,
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : IQueryHandler<GetTransactionsListQuery, PagedResultDto<TransactionDto>>
{
    public async ValueTask<PagedResultDto<TransactionDto>> Handle(GetTransactionsListQuery query, CancellationToken cancellationToken)
    {
        await SourceRevenuAuthorizationGuard.EnsureSourceRevenuOwnershipAsync(
            query.SourceRevenuId, sourceRevenuRepository, currentUserService, cancellationToken);

        var transactions = await transactionRepository.GetBySourceRevenuIdAsync(query.SourceRevenuId, query.Page, query.PageSize, cancellationToken);

        return new PagedResultDto<TransactionDto>(
            transactions.Items.Select(t => t.ToDto()).ToList(),
            transactions.TotalCount,
            transactions.Page,
            transactions.PageSize);
    }
}
