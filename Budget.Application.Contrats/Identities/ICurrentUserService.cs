namespace Budget.Application.Contrats.Identities;

public interface ICurrentUserService
{
    Guid? UtilisateurId { get; }

    string? Email { get; }

    bool EstAuthentifie { get; }
}
