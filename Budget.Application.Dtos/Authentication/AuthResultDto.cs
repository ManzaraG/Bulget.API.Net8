namespace Budget.Application.Dtos.Authentication;

public sealed record AuthResultDto(string Token, DateTime Expiration, UtilisateurDto Utilisateur);
