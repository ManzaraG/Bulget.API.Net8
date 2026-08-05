using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Persistence.Repositories;

public sealed class CompteRepository(BudgetDbContext dbContext) : ICompteRepository
{
    public async Task<Compte?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Comptes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Compte>> GetByUtilisateurIdAsync(Guid utilisateurId, CancellationToken cancellationToken = default)
        => await dbContext.Comptes
            .Where(c => c.UtilisateurId == utilisateurId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Compte compte, CancellationToken cancellationToken = default)
    {
        await dbContext.Comptes.AddAsync(compte, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Compte compte, CancellationToken cancellationToken = default)
    {
        dbContext.Comptes.Update(compte);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var compte = await dbContext.Comptes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (compte is null)
            return;

        dbContext.Comptes.Remove(compte);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
