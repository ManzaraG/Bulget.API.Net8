using Budget.Application.Dtos.Transactions;

namespace Budget.Application.Dtos.Dashboard;

public sealed record TransactionRecenteDto(
    Guid Id,
    decimal Montant,
    TypeTransactionDto Type,
    DateTime Date,
    string? Description,
    Guid? CategorieId,
    string CategorieNom,
    IReadOnlyList<string> SourcesRevenu);
