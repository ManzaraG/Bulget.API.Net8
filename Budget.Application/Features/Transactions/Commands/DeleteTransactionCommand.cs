using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Features.SourcesRevenu;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Transactions.Commands;

public sealed record DeleteTransactionCommand(Guid Id) : ICommand;

public sealed class DeleteTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : ICommandHandler<DeleteTransactionCommand>
{
    public async ValueTask<Unit> Handle(DeleteTransactionCommand command, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Transaction), command.Id);

        foreach (var sourceRevenuId in transaction.Repartitions.Select(r => r.SourceRevenuId).Distinct())
        {
            await SourceRevenuAuthorizationGuard.EnsureSourceRevenuOwnershipAsync(
                sourceRevenuId, sourceRevenuRepository, currentUserService, cancellationToken);
        }

        await transactionRepository.DeleteAsync(command.Id, cancellationToken);

        return Unit.Value;
    }
}
