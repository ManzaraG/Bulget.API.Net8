using Budget.Domain.DomainEnums;

namespace Budget.Domain.Entities;

public class Categorie
{
    public Guid Id { get; }
    public string Nom { get; private set; }
    public TypeTransaction Type { get; }

    private Categorie()
    {
        Nom = null!;
    }

    public Categorie(string nom, TypeTransaction type)
    {
        GuardNom(nom);

        Id = Guid.NewGuid();
        Nom = nom;
        Type = type;
    }

    public void Renommer(string nom)
    {
        GuardNom(nom);

        Nom = nom;
    }

    private static void GuardNom(string nom)
    {
        if (string.IsNullOrWhiteSpace(nom))
            throw new ArgumentException("Le nom de la catégorie est requis.", nameof(nom));
    }
}
