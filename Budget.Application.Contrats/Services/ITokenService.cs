using Budget.Domain.Entities;

namespace Budget.Application.Contrats.Services;

public interface ITokenService
{
    string GenerateToken(Utilisateur utilisateur);
}
