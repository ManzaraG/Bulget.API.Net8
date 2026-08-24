using Budget.Application.Contrats;
using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Persistence.Repositories;

public sealed class DeviseRepository(BudgetDbContext dbContext) : IDeviseRepository
{
    public async Task<Devise?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Devises.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    public async Task<PagedResult<Devise>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Devises.OrderBy(d => d.Nom);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Devise>(items, totalCount, page, pageSize);
    }

    public async Task AddAsync(Devise devise, CancellationToken cancellationToken = default)
    {
        await dbContext.Devises.AddAsync(devise, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Devise devise, CancellationToken cancellationToken = default)
    {
        dbContext.Devises.Update(devise);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var devise = await dbContext.Devises.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (devise is null)
            return;

        dbContext.Devises.Remove(devise);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
