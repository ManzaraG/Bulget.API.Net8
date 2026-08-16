namespace Budget.Domain.Entities;

public class TypeSourceRevenu
{
    public Guid Id { get; }
    public string Nom { get; private set; }

    private TypeSourceRevenu()
    {
        Nom = null!;
    }

    public TypeSourceRevenu(string nom)
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
            throw new ArgumentException("Le nom du type de source de revenu est requis.", nameof(nom));
    }
}
