namespace Budget.Domain.Entities;

public class Utilisateur
{
    public Guid Id { get; }
    public string Nom { get; private set; }
    public string Email { get; private set; }
    public string MotDePasseHash { get; private set; }
    public DateTime DateCreation { get; }

    private Utilisateur()
    {
        Nom = null!;
        Email = null!;
        MotDePasseHash = null!;
    }

    public Utilisateur(string nom, string email, string motDePasseHash)
    {
        if (string.IsNullOrWhiteSpace(nom))
            throw new ArgumentException("Le nom de l'utilisateur est requis.", nameof(nom));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("L'email de l'utilisateur est requis.", nameof(email));

        if (string.IsNullOrWhiteSpace(motDePasseHash))
            throw new ArgumentException("Le mot de passe de l'utilisateur est requis.", nameof(motDePasseHash));

        Id = Guid.NewGuid();
        Nom = nom;
        Email = email;
        MotDePasseHash = motDePasseHash;
        DateCreation = DateTime.UtcNow;
    }
}
