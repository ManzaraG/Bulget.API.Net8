using Budget.Application.Contrats;
using Budget.Domain.Entities;

namespace Budget.Application.Contrats.Repositories;

public interface ISourceRevenuRepository
{
    Task<SourceRevenu?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PagedResult<SourceRevenu>> GetByUtilisateurIdAsync(Guid utilisateurId, int page, int pageSize, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SourceRevenu>> GetByTypeIdAsync(Guid typeId, CancellationToken cancellationToken = default);

    Task AddAsync(SourceRevenu sourceRevenu, CancellationToken cancellationToken = default);

    Task UpdateAsync(SourceRevenu sourceRevenu, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
