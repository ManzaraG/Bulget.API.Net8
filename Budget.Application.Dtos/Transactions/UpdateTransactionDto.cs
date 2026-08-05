namespace Budget.Application.Dtos.Transactions;

public sealed record UpdateTransactionDto(
    decimal Montant,
    string? Description,
    Guid? CategorieId,
    DateTime Date);
