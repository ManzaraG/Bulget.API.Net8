using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Transactions;
using Budget.Application.Features.SourcesRevenu;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Transactions.Commands;

public sealed record UpdateTransactionCommand(
    Guid Id,
    IReadOnlyList<RepartitionSourceRevenuDto> Repartitions,
    string? Description,
    Guid? CategorieId,
    DateTime Date) : ICommand<TransactionDto>;

public sealed class UpdateTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : ICommandHandler<UpdateTransactionCommand, TransactionDto>
{
    public async ValueTask<TransactionDto> Handle(UpdateTransactionCommand command, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Transaction), command.Id);

        var sourceRevenuIdsAVerifier = transaction.Repartitions.Select(r => r.SourceRevenuId)
            .Concat(command.Repartitions.Select(r => r.SourceRevenuId))
            .Distinct();

        foreach (var sourceRevenuId in sourceRevenuIdsAVerifier)
        {
            await SourceRevenuAuthorizationGuard.EnsureSourceRevenuOwnershipAsync(
                sourceRevenuId, sourceRevenuRepository, currentUserService, cancellationToken);
        }

        var repartitions = command.Repartitions
            .Select(r => new RepartitionSourceRevenu(r.SourceRevenuId, r.Montant))
            .ToList();

        transaction.Modifier(repartitions, command.Description, command.CategorieId, command.Date);

        await transactionRepository.UpdateAsync(transaction, cancellationToken);

        return transaction.ToDto();
    }
}
