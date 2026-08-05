using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Comptes.Commands;

public sealed record DeleteCompteCommand(Guid Id) : ICommand;

public sealed class DeleteCompteCommandHandler(
    ICompteRepository compteRepository,
    ICurrentUserService currentUserService) : ICommandHandler<DeleteCompteCommand>
{
    public async ValueTask<Unit> Handle(DeleteCompteCommand command, CancellationToken cancellationToken)
    {
        var compte = await compteRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Compte), command.Id);

        CompteAuthorizationGuard.EnsureOwnership(compte, currentUserService);

        await compteRepository.DeleteAsync(command.Id, cancellationToken);

        return Unit.Value;
    }
}
