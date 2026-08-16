using Budget.Application.Contrats;
using Budget.Domain.Entities;

namespace Budget.Application.Contrats.Repositories;

public interface ICategorieRepository
{
    Task<Categorie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PagedResult<Categorie>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    Task AddAsync(Categorie categorie, CancellationToken cancellationToken = default);

    Task UpdateAsync(Categorie categorie, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
