using Budget.Domain.Entities;

namespace Budget.Application.Contrats.Services;

public interface ITokenService
{
    TokenResult GenerateToken(Utilisateur utilisateur, bool seSouvenirDeMoi = false);
}

public sealed record TokenResult(string Token, DateTime Expiration);
