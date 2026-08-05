using Budget.Application.Contrats.Repositories;
using Budget.Application.Contrats.Services;
using Budget.Application.Dtos.Authentication;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Authentication.Commands;

public sealed record LoginCommand(string Email, string MotDePasse) : ICommand<AuthResultDto>;

public sealed class LoginCommandHandler(
    IUtilisateurRepository utilisateurRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : ICommandHandler<LoginCommand, AuthResultDto>
{
    public async ValueTask<AuthResultDto> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var utilisateur = await utilisateurRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (utilisateur is null || !passwordHasher.Verify(utilisateur.MotDePasseHash, command.MotDePasse))
            throw new ForbiddenAccessException("Email ou mot de passe incorrect.");

        var token = tokenService.GenerateToken(utilisateur);

        return new AuthResultDto(token, utilisateur.Id, utilisateur.Nom, utilisateur.Email);
    }
}
