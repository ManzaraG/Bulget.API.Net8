namespace Budget.Application.Dtos.Authentication;

public sealed record AuthResultDto(string Token, Guid UtilisateurId, string Nom, string Email);
