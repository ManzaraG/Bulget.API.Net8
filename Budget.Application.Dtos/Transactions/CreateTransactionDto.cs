namespace Budget.Application.Dtos.Transactions;

public sealed record CreateTransactionDto(
    decimal Montant,
    TypeTransactionDto Type,
    Guid CompteId,
    Guid? CategorieId,
    string? Description,
    DateTime? Date);
