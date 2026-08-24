using Budget.Domain.DomainEnums;

namespace Budget.Domain.Entities;

public class Transaction
{
    public Guid Id { get; }
    public TypeTransaction Type { get; }
    public DateTime Date { get; private set; }
    public string? Description { get; private set; }
    public Guid? CategorieId { get; private set; }

    private readonly List<RepartitionSourceRevenu> _repartitions = new();
    public IReadOnlyList<RepartitionSourceRevenu> Repartitions => _repartitions;

    public decimal Montant => _repartitions.Sum(r => r.Montant);

    public bool EstRevenu => Type == TypeTransaction.Revenu;
    public bool EstDepense => Type == TypeTransaction.Depense;

    private Transaction()
    {
    }

    private Transaction(
        TypeTransaction type,
        IReadOnlyList<RepartitionSourceRevenu> repartitions,
        Guid? categorieId,
        string? description,
        DateTime date)
    {
        GuardRepartitions(repartitions);

        Id = Guid.NewGuid();
        Type = type;
        CategorieId = categorieId;
        Description = description;
        Date = date;

        AffecterRepartitions(repartitions);
    }

    public static Transaction CreerRevenu(
        IReadOnlyList<RepartitionSourceRevenu> repartitions,
        Guid? categorieId = null,
        string? description = null,
        DateTime? date = null)
        => new(TypeTransaction.Revenu, repartitions, categorieId, description, date ?? DateTime.UtcNow);

    public static Transaction CreerDepense(
        IReadOnlyList<RepartitionSourceRevenu> repartitions,
        Guid? categorieId = null,
        string? description = null,
        DateTime? date = null)
        => new(TypeTransaction.Depense, repartitions, categorieId, description, date ?? DateTime.UtcNow);

    public void Modifier(IReadOnlyList<RepartitionSourceRevenu> repartitions, string? description, Guid? categorieId, DateTime date)
    {
        GuardRepartitions(repartitions);

        _repartitions.Clear();
        AffecterRepartitions(repartitions);

        Description = description;
        CategorieId = categorieId;
        Date = date;
    }

    private void AffecterRepartitions(IReadOnlyList<RepartitionSourceRevenu> repartitions)
    {
        foreach (var repartition in repartitions)
        {
            repartition.RattacherATransaction(Id);
            _repartitions.Add(repartition);
        }
    }

    private static void GuardRepartitions(IReadOnlyList<RepartitionSourceRevenu> repartitions)
    {
        if (repartitions is null || repartitions.Count == 0)
            throw new ArgumentException("Une transaction doit être rattachée à au moins une source de revenu.", nameof(repartitions));

        var sourceRevenuIds = repartitions.Select(r => r.SourceRevenuId).ToList();
        if (sourceRevenuIds.Distinct().Count() != sourceRevenuIds.Count)
            throw new ArgumentException("Une source de revenu ne peut apparaître qu'une seule fois dans la répartition d'une transaction.", nameof(repartitions));
    }
}
