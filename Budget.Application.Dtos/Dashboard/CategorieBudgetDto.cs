namespace Budget.Application.Dtos.Dashboard;

public sealed record CategorieBudgetDto(
    Guid CategorieId,
    string Nom,
    decimal Montant,
    decimal Pourcentage);
