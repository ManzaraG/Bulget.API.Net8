using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Transactions;
using Budget.Application.Features.SourcesRevenu;
using Budget.Domain.Entities;
using Mediator;

namespace Budget.Application.Features.Transactions.Commands;

public sealed record CreateTransactionCommand(
    decimal Montant,
    TypeTransactionDto Type,
    Guid SourceRevenuId,
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
        await SourceRevenuAuthorizationGuard.EnsureSourceRevenuOwnershipAsync(
            command.SourceRevenuId, sourceRevenuRepository, currentUserService, cancellationToken);

        var transaction = command.Type == TypeTransactionDto.Revenu
            ? Transaction.CreerRevenu(command.Montant, command.SourceRevenuId, command.CategorieId, command.Description, command.Date)
            : Transaction.CreerDepense(command.Montant, command.SourceRevenuId, command.CategorieId, command.Description, command.Date);

        await transactionRepository.AddAsync(transaction, cancellationToken);

        return transaction.ToDto();
    }
}
