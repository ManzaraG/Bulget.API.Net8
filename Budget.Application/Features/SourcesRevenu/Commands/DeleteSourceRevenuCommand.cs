using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.SourcesRevenu.Commands;

public sealed record DeleteSourceRevenuCommand(Guid Id) : ICommand;

public sealed class DeleteSourceRevenuCommandHandler(
    ISourceRevenuRepository sourceRevenuRepository,
    ICurrentUserService currentUserService) : ICommandHandler<DeleteSourceRevenuCommand>
{
    public async ValueTask<Unit> Handle(DeleteSourceRevenuCommand command, CancellationToken cancellationToken)
    {
        var sourceRevenu = await sourceRevenuRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(SourceRevenu), command.Id);

        SourceRevenuAuthorizationGuard.EnsureOwnership(sourceRevenu, currentUserService);

        if (sourceRevenu.EstActif)
            throw new ConflictException("Impossible de supprimer une source de revenu active. Désactivez-la d'abord.");

        await sourceRevenuRepository.DeleteAsync(command.Id, cancellationToken);

        return Unit.Value;
    }
}
