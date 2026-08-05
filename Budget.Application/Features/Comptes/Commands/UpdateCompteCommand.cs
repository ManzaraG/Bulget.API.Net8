using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Comptes;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Comptes.Commands;

public sealed record UpdateCompteCommand(Guid Id, string Nom) : ICommand<CompteDto>;

public sealed class UpdateCompteCommandHandler(
    ICompteRepository compteRepository,
    ICurrentUserService currentUserService) : ICommandHandler<UpdateCompteCommand, CompteDto>
{
    public async ValueTask<CompteDto> Handle(UpdateCompteCommand command, CancellationToken cancellationToken)
    {
        var compte = await compteRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Compte), command.Id);

        CompteAuthorizationGuard.EnsureOwnership(compte, currentUserService);

        compte.Renommer(command.Nom);

        await compteRepository.UpdateAsync(compte, cancellationToken);

        return compte.ToDto();
    }
}
