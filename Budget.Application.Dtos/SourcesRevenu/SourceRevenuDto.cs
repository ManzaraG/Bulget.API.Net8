namespace Budget.Application.Dtos.SourcesRevenu;

public sealed record SourceRevenuDto(
    Guid Id,
    string Nom,
    TypeSourceRevenuDto Type,
    bool EstActif,
    Guid UtilisateurId,
    DateTime DateCreation);
