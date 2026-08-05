using Budget.Domain.Entities;

namespace Budget.Application.Contrats.Repositories;

public interface IUtilisateurRepository
{
    Task<Utilisateur?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Utilisateur?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task AddAsync(Utilisateur utilisateur, CancellationToken cancellationToken = default);
}
