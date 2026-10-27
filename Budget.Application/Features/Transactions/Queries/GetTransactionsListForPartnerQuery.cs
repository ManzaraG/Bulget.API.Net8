using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Common;
using Budget.Application.Dtos.Transactions;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Transactions.Queries;

public sealed record GetTransactionsListForPartnerQuery(Guid SourceRevenuId, int Page, int PageSize) : IQuery<PagedResultDto<TransactionDto>>;

public sealed class GetTransactionsListForPartnerQueryHandler(
    ITransactionRepository transactionRepository,
    ISourceRevenuRepository sourceRevenuRepository) : IQueryHandler<GetTransactionsListForPartnerQuery, PagedResultDto<TransactionDto>>
{
    public async ValueTask<PagedResultDto<TransactionDto>> Handle(GetTransactionsListForPartnerQuery query, CancellationToken cancellationToken)
    {
        _ = await sourceRevenuRepository.GetByIdAsync(query.SourceRevenuId, cancellationToken)
            ?? throw new NotFoundException(nameof(SourceRevenu), query.SourceRevenuId);

        var transactions = await transactionRepository.GetBySourceRevenuIdAsync(query.SourceRevenuId, query.Page, query.PageSize, cancellationToken);

        return new PagedResultDto<TransactionDto>(
            transactions.Items.Select(t => t.ToDto()).ToList(),
            transactions.TotalCount,
            transactions.Page,
            transactions.PageSize);
    }
}
