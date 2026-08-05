namespace Budget.Application.Dtos.Comptes;

public sealed record CompteDto(
    Guid Id,
    string Nom,
    Guid UtilisateurId,
    DateTime DateCreation);
