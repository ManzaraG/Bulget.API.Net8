using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Persistence.Repositories;

public sealed class UtilisateurRepository(BudgetDbContext dbContext) : IUtilisateurRepository
{
    public async Task<Utilisateur?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Utilisateurs.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<Utilisateur?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await dbContext.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task AddAsync(Utilisateur utilisateur, CancellationToken cancellationToken = default)
    {
        await dbContext.Utilisateurs.AddAsync(utilisateur, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
