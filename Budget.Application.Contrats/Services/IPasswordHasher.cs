namespace Budget.Application.Contrats.Services;

public interface IPasswordHasher
{
    string Hash(string motDePasse);

    bool Verify(string motDePasseHash, string motDePasse);
}
