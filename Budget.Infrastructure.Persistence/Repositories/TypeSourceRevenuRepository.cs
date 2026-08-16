using Budget.Application.Contrats;
using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Persistence.Repositories;

public sealed class TypeSourceRevenuRepository(BudgetDbContext dbContext) : ITypeSourceRevenuRepository
{
    public async Task<TypeSourceRevenu?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.TypesSourceRevenu.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<PagedResult<TypeSourceRevenu>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = dbContext.TypesSourceRevenu.OrderBy(t => t.Nom);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TypeSourceRevenu>(items, totalCount, page, pageSize);
    }

    public async Task AddAsync(TypeSourceRevenu typeSourceRevenu, CancellationToken cancellationToken = default)
    {
        await dbContext.TypesSourceRevenu.AddAsync(typeSourceRevenu, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TypeSourceRevenu typeSourceRevenu, CancellationToken cancellationToken = default)
    {
        dbContext.TypesSourceRevenu.Update(typeSourceRevenu);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var typeSourceRevenu = await dbContext.TypesSourceRevenu.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (typeSourceRevenu is null)
            return;

        dbContext.TypesSourceRevenu.Remove(typeSourceRevenu);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
