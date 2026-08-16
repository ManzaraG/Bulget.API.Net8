using Budget.Application.Contrats;
using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Persistence.Repositories;

public sealed class SourceRevenuRepository(BudgetDbContext dbContext) : ISourceRevenuRepository
{
    public async Task<SourceRevenu?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.SourcesRevenu.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<PagedResult<SourceRevenu>> GetByUtilisateurIdAsync(Guid utilisateurId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = dbContext.SourcesRevenu
            .Where(s => s.UtilisateurId == utilisateurId)
            .OrderBy(s => s.Nom);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<SourceRevenu>(items, totalCount, page, pageSize);
    }

    public async Task<IReadOnlyList<SourceRevenu>> GetByTypeIdAsync(Guid typeId, CancellationToken cancellationToken = default)
        => await dbContext.SourcesRevenu
            .Where(s => s.TypeId == typeId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(SourceRevenu sourceRevenu, CancellationToken cancellationToken = default)
    {
        await dbContext.SourcesRevenu.AddAsync(sourceRevenu, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SourceRevenu sourceRevenu, CancellationToken cancellationToken = default)
    {
        dbContext.SourcesRevenu.Update(sourceRevenu);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sourceRevenu = await dbContext.SourcesRevenu.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (sourceRevenu is null)
            return;

        dbContext.SourcesRevenu.Remove(sourceRevenu);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
