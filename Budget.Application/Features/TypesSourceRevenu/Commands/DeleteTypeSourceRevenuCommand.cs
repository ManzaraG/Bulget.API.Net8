using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.TypesSourceRevenu.Commands;

public sealed record DeleteTypeSourceRevenuCommand(Guid Id) : ICommand;

public sealed class DeleteTypeSourceRevenuCommandHandler(
    ITypeSourceRevenuRepository typeSourceRevenuRepository,
    ISourceRevenuRepository sourceRevenuRepository) : ICommandHandler<DeleteTypeSourceRevenuCommand>
{
    public async ValueTask<Unit> Handle(DeleteTypeSourceRevenuCommand command, CancellationToken cancellationToken)
    {
        _ = await typeSourceRevenuRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(TypeSourceRevenu), command.Id);

        var sourcesRevenuUtilisantCeType = await sourceRevenuRepository.GetByTypeIdAsync(command.Id, cancellationToken);
        if (sourcesRevenuUtilisantCeType.Count > 0)
            throw new ConflictException("Impossible de supprimer un type encore utilisé par une source de revenu.");

        await typeSourceRevenuRepository.DeleteAsync(command.Id, cancellationToken);

        return Unit.Value;
    }
}
