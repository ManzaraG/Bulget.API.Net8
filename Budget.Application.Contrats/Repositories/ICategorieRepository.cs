using Budget.Domain.Entities;

namespace Budget.Application.Contrats.Repositories;

public interface ICategorieRepository
{
    Task<Categorie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Categorie>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Categorie categorie, CancellationToken cancellationToken = default);

    Task UpdateAsync(Categorie categorie, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
