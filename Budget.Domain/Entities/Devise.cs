namespace Budget.Domain.Entities;

public class Devise
{
    public Guid Id { get; }
    public string Nom { get; private set; }

    private Devise()
    {
        Nom = null!;
    }

    public Devise(string nom)
    {
        GuardNom(nom);

        Id = Guid.NewGuid();
        Nom = nom;
    }

    public void Renommer(string nom)
    {
        GuardNom(nom);

        Nom = nom;
    }

    private static void GuardNom(string nom)
    {
        if (string.IsNullOrWhiteSpace(nom))
            throw new ArgumentException("Le nom de la devise est requis.", nameof(nom));
    }
}
