namespace Budget.Application.Dtos.Authentication;

public sealed record RegisterDto(
    string Prenom,
    string Nom,
    string Email,
    string MotDePasse,
    string ConfirmationMotDePasse,
    bool AccepteConditions);
