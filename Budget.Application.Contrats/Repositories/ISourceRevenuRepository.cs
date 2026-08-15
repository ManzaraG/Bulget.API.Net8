using Budget.Domain.Entities;

namespace Budget.Application.Contrats.Repositories;

public interface ISourceRevenuRepository
{
    Task<SourceRevenu?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SourceRevenu>> GetByUtilisateurIdAsync(Guid utilisateurId, CancellationToken cancellationToken = default);

    Task AddAsync(SourceRevenu sourceRevenu, CancellationToken cancellationToken = default);

    Task UpdateAsync(SourceRevenu sourceRevenu, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
