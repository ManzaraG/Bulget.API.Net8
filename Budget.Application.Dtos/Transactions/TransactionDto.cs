namespace Budget.Application.Dtos.Transactions;

public sealed record TransactionDto(
    Guid Id,
    decimal Montant,
    TypeTransactionDto Type,
    DateTime Date,
    string? Description,
    Guid SourceRevenuId,
    Guid? CategorieId);
