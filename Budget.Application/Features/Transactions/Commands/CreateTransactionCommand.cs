using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Transactions;
using Budget.Application.Features.SourcesRevenu;
using Budget.Domain.Entities;
using Mediator;

namespace Budget.Application.Features.Transactions.Commands;

public sealed record CreateTransactionCommand(
    TypeTransactionDto Type,
    IReadOnlyList<RepartitionSourceRevenuDto> Repartitions,
    Guid? CategorieId,
    string? Description,
    DateTime? Date) : ICommand<TransactionDto>;

public sealed class CreateTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : ICommandHandler<CreateTransactionCommand, TransactionDto>
{
    public async ValueTask<TransactionDto> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        foreach (var sourceRevenuId in command.Repartitions.Select(r => r.SourceRevenuId).Distinct())
        {
            await SourceRevenuAuthorizationGuard.EnsureSourceRevenuOwnershipAsync(
                sourceRevenuId, sourceRevenuRepository, currentUserService, cancellationToken);
        }

        var repartitions = command.Repartitions
            .Select(r => new RepartitionSourceRevenu(r.SourceRevenuId, r.Montant))
            .ToList();

        var transaction = command.Type == TypeTransactionDto.Revenu
            ? Transaction.CreerRevenu(repartitions, command.CategorieId, command.Description, command.Date)
            : Transaction.CreerDepense(repartitions, command.CategorieId, command.Description, command.Date);

        await transactionRepository.AddAsync(transaction, cancellationToken);

        return transaction.ToDto();
    }
}
