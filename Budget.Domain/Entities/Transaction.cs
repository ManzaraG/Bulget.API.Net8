using Budget.Domain.DomainEnums;

namespace Budget.Domain.Entities;

public class Transaction
{
    public Guid Id { get; }
    public decimal Montant { get; private set; }
    public TypeTransaction Type { get; }
    public DateTime Date { get; private set; }
    public string? Description { get; private set; }
    public Guid SourceRevenuId { get; }
    public Guid? CategorieId { get; private set; }

    public bool EstRevenu => Type == TypeTransaction.Revenu;
    public bool EstDepense => Type == TypeTransaction.Depense;

    private Transaction()
    {
    }

    private Transaction(
        decimal montant,
        TypeTransaction type,
        Guid sourceRevenuId,
        Guid? categorieId,
        string? description,
        DateTime date)
    {
        GuardMontantPositif(montant);

        if (sourceRevenuId == Guid.Empty)
            throw new ArgumentException("Une transaction doit être rattachée à une source de revenu.", nameof(sourceRevenuId));

        Id = Guid.NewGuid();
        Montant = montant;
        Type = type;
        SourceRevenuId = sourceRevenuId;
        CategorieId = categorieId;
        Description = description;
        Date = date;
    }

    public static Transaction CreerRevenu(
        decimal montant,
        Guid sourceRevenuId,
        Guid? categorieId = null,
        string? description = null,
        DateTime? date = null)
        => new(montant, TypeTransaction.Revenu, sourceRevenuId, categorieId, description, date ?? DateTime.UtcNow);

    public static Transaction CreerDepense(
        decimal montant,
        Guid sourceRevenuId,
        Guid? categorieId = null,
        string? description = null,
        DateTime? date = null)
        => new(montant, TypeTransaction.Depense, sourceRevenuId, categorieId, description, date ?? DateTime.UtcNow);

    public void Modifier(decimal montant, string? description, Guid? categorieId, DateTime date)
    {
        GuardMontantPositif(montant);

        Montant = montant;
        Description = description;
        CategorieId = categorieId;
        Date = date;
    }

    private static void GuardMontantPositif(decimal montant)
    {
        if (montant <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(montant),
                montant,
                "Le montant d'une transaction doit être strictement positif.");
    }
}
