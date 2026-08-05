using Budget.Domain.Entities;

namespace Budget.Application.Contrats.Repositories;

public interface ICompteRepository
{
    Task<Compte?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Compte>> GetByUtilisateurIdAsync(Guid utilisateurId, CancellationToken cancellationToken = default);

    Task AddAsync(Compte compte, CancellationToken cancellationToken = default);

    Task UpdateAsync(Compte compte, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
