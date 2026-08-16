using Budget.Application.Contrats;
using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Persistence.Repositories;

public sealed class CategorieRepository(BudgetDbContext dbContext) : ICategorieRepository
{
    public async Task<Categorie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<PagedResult<Categorie>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Categories.OrderBy(c => c.Nom);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Categorie>(items, totalCount, page, pageSize);
    }

    public async Task AddAsync(Categorie categorie, CancellationToken cancellationToken = default)
    {
        await dbContext.Categories.AddAsync(categorie, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Categorie categorie, CancellationToken cancellationToken = default)
    {
        dbContext.Categories.Update(categorie);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var categorie = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (categorie is null)
            return;

        dbContext.Categories.Remove(categorie);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
