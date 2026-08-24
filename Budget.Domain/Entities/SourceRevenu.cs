namespace Budget.Domain.Entities;

public class SourceRevenu
{
    public Guid Id { get; }
    public string Nom { get; private set; }
    public Guid TypeId { get; private set; }
    public Guid DeviseId { get; private set; }
    public bool EstActif { get; private set; }
    public Guid UtilisateurId { get; }
    public DateTime DateCreation { get; }

    private SourceRevenu()
    {
        Nom = null!;
    }

    public SourceRevenu(string nom, Guid typeId, Guid deviseId, Guid utilisateurId)
    {
        GuardNom(nom);
        GuardTypeId(typeId);
        GuardDeviseId(deviseId);

        if (utilisateurId == Guid.Empty)
            throw new ArgumentException("Une source de revenu doit appartenir à un utilisateur.", nameof(utilisateurId));

        Id = Guid.NewGuid();
        Nom = nom;
        TypeId = typeId;
        DeviseId = deviseId;
        EstActif = true;
        UtilisateurId = utilisateurId;
        DateCreation = DateTime.UtcNow;
    }

    public void Renommer(string nom)
    {
        GuardNom(nom);

        Nom = nom;
    }

    public void ChangerType(Guid typeId)
    {
        GuardTypeId(typeId);

        TypeId = typeId;
    }

    public void ChangerDevise(Guid deviseId)
    {
        GuardDeviseId(deviseId);

        DeviseId = deviseId;
    }

    public void Activer()
    {
        EstActif = true;
    }

    public void Desactiver()
    {
        EstActif = false;
    }

    private static void GuardNom(string nom)
    {
        if (string.IsNullOrWhiteSpace(nom))
            throw new ArgumentException("Le nom de la source de revenu est requis.", nameof(nom));
    }

    private static void GuardTypeId(Guid typeId)
    {
        if (typeId == Guid.Empty)
            throw new ArgumentException("Une source de revenu doit avoir un type.", nameof(typeId));
    }

    private static void GuardDeviseId(Guid deviseId)
    {
        if (deviseId == Guid.Empty)
            throw new ArgumentException("Une source de revenu doit avoir une devise.", nameof(deviseId));
    }
}
