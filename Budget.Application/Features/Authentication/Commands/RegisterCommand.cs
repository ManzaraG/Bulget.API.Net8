using System.Text.RegularExpressions;
using Budget.Application.Contrats.Repositories;
using Budget.Application.Contrats.Services;
using Budget.Application.Dtos.Authentication;
using Budget.Domain.Entities;
using Budget.Infrastructure.Exceptions;
using Mediator;

namespace Budget.Application.Features.Authentication.Commands;

public sealed record RegisterCommand(
    string Prenom,
    string Nom,
    string Email,
    string MotDePasse,
    string ConfirmationMotDePasse,
    bool AccepteConditions) : ICommand<AuthResultDto>;

public sealed partial class RegisterCommandHandler(
    IUtilisateurRepository utilisateurRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : ICommandHandler<RegisterCommand, AuthResultDto>
{
    public async ValueTask<AuthResultDto> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (!command.AccepteConditions)
            throw new ArgumentException("Vous devez accepter les conditions d'utilisation.", nameof(command.AccepteConditions));

        if (!MotDePasseRegex().IsMatch(command.MotDePasse))
            throw new ArgumentException(
                "Le mot de passe doit contenir au moins 8 caractères, une majuscule, une minuscule, un chiffre et un caractère spécial.",
                nameof(command.MotDePasse));

        if (command.ConfirmationMotDePasse != command.MotDePasse)
            throw new ArgumentException("La confirmation du mot de passe ne correspond pas.", nameof(command.ConfirmationMotDePasse));

        var utilisateurExistant = await utilisateurRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (utilisateurExistant is not null)
            throw new ConflictException("Cet email est déjà utilisé.");

        var motDePasseHash = passwordHasher.Hash(command.MotDePasse);
        var utilisateur = new Utilisateur(command.Prenom, command.Nom, command.Email, motDePasseHash);

        await utilisateurRepository.AddAsync(utilisateur, cancellationToken);

        var tokenResult = tokenService.GenerateToken(utilisateur);

        return new AuthResultDto(
            tokenResult.Token,
            tokenResult.Expiration,
            new UtilisateurDto(utilisateur.Id, utilisateur.Prenom, utilisateur.Nom, utilisateur.Email));
    }

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$")]
    private static partial Regex MotDePasseRegex();
}
