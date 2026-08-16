using Budget.Application.Contrats;
using Budget.Domain.Entities;

namespace Budget.Application.Contrats.Repositories;

public interface ITypeSourceRevenuRepository
{
    Task<TypeSourceRevenu?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PagedResult<TypeSourceRevenu>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    Task AddAsync(TypeSourceRevenu typeSourceRevenu, CancellationToken cancellationToken = default);

    Task UpdateAsync(TypeSourceRevenu typeSourceRevenu, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
