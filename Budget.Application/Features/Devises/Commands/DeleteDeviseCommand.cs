using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Devises.Commands;

public sealed record DeleteDeviseCommand(Guid Id) : ICommand;

public sealed class DeleteDeviseCommandHandler(IDeviseRepository deviseRepository) : ICommandHandler<DeleteDeviseCommand>
{
    public async ValueTask<Unit> Handle(DeleteDeviseCommand command, CancellationToken cancellationToken)
    {
        _ = await deviseRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Devise), command.Id);

        await deviseRepository.DeleteAsync(command.Id, cancellationToken);

        return Unit.Value;
    }
}
