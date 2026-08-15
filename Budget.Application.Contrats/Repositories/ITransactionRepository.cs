using Budget.Domain.Entities;

namespace Budget.Application.Contrats.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Transaction>> GetBySourceRevenuIdAsync(Guid sourceRevenuId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Transaction>> GetByUtilisateurIdAndPeriodAsync(
        Guid utilisateurId,
        DateTime debut,
        DateTime fin,
        CancellationToken cancellationToken = default);

    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);

    Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
