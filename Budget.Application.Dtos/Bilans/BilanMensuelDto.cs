namespace Budget.Application.Dtos.Bilans;

public sealed record BilanMensuelDto(
    Guid UtilisateurId,
    int Annee,
    int Mois,
    decimal TotalRevenus,
    decimal TotalDepenses,
    decimal Solde);
