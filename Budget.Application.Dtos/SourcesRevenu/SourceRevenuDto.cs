namespace Budget.Application.Dtos.SourcesRevenu;

public sealed record SourceRevenuDto(
    Guid Id,
    string Nom,
    Guid TypeId,
    bool EstActif,
    Guid UtilisateurId,
    DateTime DateCreation);
