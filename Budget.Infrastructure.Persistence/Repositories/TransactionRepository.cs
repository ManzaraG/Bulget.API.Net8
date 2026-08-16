using Budget.Application.Contrats;
using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Persistence.Repositories;

public sealed class TransactionRepository(BudgetDbContext dbContext) : ITransactionRepository
{
    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<PagedResult<Transaction>> GetBySourceRevenuIdAsync(Guid sourceRevenuId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Transactions
            .Where(t => t.SourceRevenuId == sourceRevenuId)
            .OrderByDescending(t => t.Date);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Transaction>(items, totalCount, page, pageSize);
    }

    public async Task<IReadOnlyList<Transaction>> GetByUtilisateurIdAndPeriodAsync(
        Guid utilisateurId,
        DateTime debut,
        DateTime fin,
        CancellationToken cancellationToken = default)
    {
        var sourceRevenuIds = dbContext.SourcesRevenu
            .Where(s => s.UtilisateurId == utilisateurId)
            .Select(s => s.Id);

        return await dbContext.Transactions
            .Where(t => sourceRevenuIds.Contains(t.SourceRevenuId) && t.Date >= debut && t.Date < fin)
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
