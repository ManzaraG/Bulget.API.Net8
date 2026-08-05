using Budget.Application.Dtos.Transactions;

namespace Budget.Application.Dtos.Categories;

public sealed record CategorieDto(Guid Id, string Nom, TypeTransactionDto Type);
