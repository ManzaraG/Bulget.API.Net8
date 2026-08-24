namespace Budget.Application.Dtos.Dashboard;

public sealed record DashboardTrendPointDto(
    int Annee,
    int Mois,
    decimal TotalRevenus,
    decimal TotalDepenses);
