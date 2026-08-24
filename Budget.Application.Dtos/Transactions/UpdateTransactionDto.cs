namespace Budget.Application.Dtos.Transactions;

public sealed record UpdateTransactionDto(
    IReadOnlyList<RepartitionSourceRevenuDto> Repartitions,
    string? Description,
    Guid? CategorieId,
    DateTime Date);
