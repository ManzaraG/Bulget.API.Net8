using Budget.Application.Dtos.Transactions;

namespace Budget.Application.Dtos.Categories;

public sealed record CreateCategorieDto(string Nom, TypeTransactionDto Type);
