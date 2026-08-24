namespace Budget.Application.Dtos.Transactions;

public sealed record TransactionDto(
    Guid Id,
    decimal Montant,
    TypeTransactionDto Type,
    DateTime Date,
    string? Description,
    IReadOnlyList<RepartitionSourceRevenuDto> Repartitions,
    Guid? CategorieId);
