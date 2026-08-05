using Budget.Application.Dtos.Categories;
using Budget.Application.Dtos.Transactions;
using Budget.Domain.DomainEnums;
using Budget.Domain.Entities;

namespace Budget.Application.Features.Categories;

internal static class CategorieMappingExtensions
{
    public static CategorieDto ToDto(this Categorie categorie) => new(
        categorie.Id,
        categorie.Nom,
        categorie.Type == TypeTransaction.Revenu ? TypeTransactionDto.Revenu : TypeTransactionDto.Depense);
}
