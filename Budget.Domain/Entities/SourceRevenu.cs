using Budget.Domain.DomainEnums;

namespace Budget.Domain.Entities;

public class SourceRevenu
{
    public Guid Id { get; }
    public string Nom { get; private set; }
    public TypeSourceRevenu Type { get; }
    public bool EstActif { get; private set; }
    public Guid UtilisateurId { get; }
    public DateTime DateCreation { get; }

    private SourceRevenu()
    {
        Nom = null!;
    }

    public SourceRevenu(string nom, TypeSourceRevenu type, Guid utilisateurId)
    {
        GuardNom(nom);

        if (utilisateurId == Guid.Empty)
            throw new ArgumentException("Une source de revenu doit appartenir à un utilisateur.", nameof(utilisateurId));

        Id = Guid.NewGuid();
        Nom = nom;
        Type = type;
        EstActif = true;
        UtilisateurId = utilisateurId;
        DateCreation = DateTime.UtcNow;
    }

    public void Renommer(string nom)
    {
        GuardNom(nom);

        Nom = nom;
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
}
