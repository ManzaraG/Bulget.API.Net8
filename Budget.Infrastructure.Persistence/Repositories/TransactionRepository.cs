using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Persistence.Repositories;

public sealed class TransactionRepository(BudgetDbContext dbContext) : ITransactionRepository
{
    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Transaction>> GetByCompteIdAsync(Guid compteId, CancellationToken cancellationToken = default)
        => await dbContext.Transactions
            .Where(t => t.CompteId == compteId)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Transaction>> GetByUtilisateurIdAndPeriodAsync(
        Guid utilisateurId,
        DateTime debut,
        DateTime fin,
        CancellationToken cancellationToken = default)
    {
        var compteIds = dbContext.Comptes
            .Where(c => c.UtilisateurId == utilisateurId)
            .Select(c => c.Id);

        return await dbContext.Transactions
            .Where(t => compteIds.Contains(t.CompteId) && t.Date >= debut && t.Date < fin)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        await dbContext.Transactions.AddAsync(transaction, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        dbContext.Transactions.Update(transaction);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (transaction is null)
            return;

        dbContext.Transactions.Remove(transaction);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
