using Budget.Application.Dtos.Transactions;
using Budget.Domain.Entities;

namespace Budget.Application.Features.Transactions;

internal static class TransactionMappingExtensions
{
    public static TransactionDto ToDto(this Transaction transaction) => new(
        transaction.Id,
        transaction.Montant,
        transaction.EstRevenu ? TypeTransactionDto.Revenu : TypeTransactionDto.Depense,
        transaction.Date,
        transaction.Description,
        transaction.Repartitions.Select(r => new RepartitionSourceRevenuDto(r.SourceRevenuId, r.Montant)).ToList(),
        transaction.CategorieId);
}
