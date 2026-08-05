using Budget.Application.Contrats.Repositories;
using Budget.Application.Contrats.Services;
using Budget.Application.Dtos.Authentication;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Authentication.Commands;

public sealed record RegisterCommand(string Nom, string Email, string MotDePasse) : ICommand<AuthResultDto>;

public sealed class RegisterCommandHandler(
    IUtilisateurRepository utilisateurRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : ICommandHandler<RegisterCommand, AuthResultDto>
{
    public async ValueTask<AuthResultDto> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var utilisateurExistant = await utilisateurRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (utilisateurExistant is not null)
            throw new ConflictException("Cet email est déjà utilisé.");

        var motDePasseHash = passwordHasher.Hash(command.MotDePasse);
        var utilisateur = new Utilisateur(command.Nom, command.Email, motDePasseHash);

        await utilisateurRepository.AddAsync(utilisateur, cancellationToken);

        var token = tokenService.GenerateToken(utilisateur);

        return new AuthResultDto(token, utilisateur.Id, utilisateur.Nom, utilisateur.Email);
    }
}
