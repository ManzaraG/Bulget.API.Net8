namespace Budget.Application.Dtos.Dashboard;

public sealed record SourceRevenuBudgetDto(
    Guid SourceRevenuId,
    string Nom,
    decimal Montant,
    decimal Pourcentage);
