using Budget.Application.Contrats;
using Budget.Domain.Entities;

namespace Budget.Application.Contrats.Repositories;

public interface IDeviseRepository
{
    Task<Devise?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PagedResult<Devise>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    Task AddAsync(Devise devise, CancellationToken cancellationToken = default);

    Task UpdateAsync(Devise devise, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
