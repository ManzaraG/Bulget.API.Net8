namespace Budget.Domain.Entities;

public class Compte
{
    public Guid Id { get; }
    public string Nom { get; private set; }
    public Guid UtilisateurId { get; }
    public DateTime DateCreation { get; }

    private Compte()
    {
        Nom = null!;
    }

    public Compte(string nom, Guid utilisateurId)
    {
        GuardNom(nom);

        if (utilisateurId == Guid.Empty)
            throw new ArgumentException("Un compte doit appartenir à un utilisateur.", nameof(utilisateurId));

        Id = Guid.NewGuid();
        Nom = nom;
        UtilisateurId = utilisateurId;
        DateCreation = DateTime.UtcNow;
    }

    public void Renommer(string nom)
    {
        GuardNom(nom);

        Nom = nom;
    }

    private static void GuardNom(string nom)
    {
        if (string.IsNullOrWhiteSpace(nom))
            throw new ArgumentException("Le nom du compte est requis.", nameof(nom));
    }
}
