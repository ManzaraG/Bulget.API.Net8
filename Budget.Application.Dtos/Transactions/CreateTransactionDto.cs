namespace Budget.Application.Dtos.Transactions;

public sealed record CreateTransactionDto(
    TypeTransactionDto Type,
    IReadOnlyList<RepartitionSourceRevenuDto> Repartitions,
    Guid? CategorieId,
    string? Description,
    DateTime? Date);
