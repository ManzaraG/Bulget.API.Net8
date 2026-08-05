using System.Text.RegularExpressions;

namespace Budget.Domain.Entities;

public partial class Utilisateur
{
    public Guid Id { get; }
    public string Prenom { get; private set; }
    public string Nom { get; private set; }
    public string Email { get; private set; }
    public string MotDePasseHash { get; private set; }
    public DateTime DateCreation { get; }

    private Utilisateur()
    {
        Prenom = null!;
        Nom = null!;
        Email = null!;
        MotDePasseHash = null!;
    }

    public Utilisateur(string prenom, string nom, string email, string motDePasseHash)
    {
        if (string.IsNullOrWhiteSpace(prenom) || prenom.Trim().Length is < 2 or > 50)
            throw new ArgumentException("Le prénom de l'utilisateur est requis et doit contenir entre 2 et 50 caractères.", nameof(prenom));

        if (string.IsNullOrWhiteSpace(nom) || nom.Trim().Length is < 2 or > 50)
            throw new ArgumentException("Le nom de l'utilisateur est requis et doit contenir entre 2 et 50 caractères.", nameof(nom));

        if (string.IsNullOrWhiteSpace(email) || !EmailRegex().IsMatch(email))
            throw new ArgumentException("L'email de l'utilisateur est requis et doit être valide.", nameof(email));

        if (string.IsNullOrWhiteSpace(motDePasseHash))
            throw new ArgumentException("Le mot de passe de l'utilisateur est requis.", nameof(motDePasseHash));

        Id = Guid.NewGuid();
        Prenom = prenom;
        Nom = nom;
        Email = email;
        MotDePasseHash = motDePasseHash;
        DateCreation = DateTime.UtcNow;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
