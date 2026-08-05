using Budget.Application.Contrats.Identities;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Dtos.Comptes;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Comptes.Commands;

public sealed record CreateCompteCommand(string Nom) : ICommand<CompteDto>;

public sealed class CreateCompteCommandHandler(
    ICompteRepository compteRepository,
    ICurrentUserService currentUserService) : ICommandHandler<CreateCompteCommand, CompteDto>
{
    public async ValueTask<CompteDto> Handle(CreateCompteCommand command, CancellationToken cancellationToken)
    {
        var utilisateurId = currentUserService.UtilisateurId
            ?? throw new ForbiddenAccessException("Utilisateur non authentifié.");

        var compte = new Compte(command.Nom, utilisateurId);

        await compteRepository.AddAsync(compte, cancellationToken);

        return compte.ToDto();
    }
}
