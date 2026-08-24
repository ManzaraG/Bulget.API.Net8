namespace Budget.Domain.Entities;

public class RepartitionSourceRevenu
{
    public Guid Id { get; }
    public Guid TransactionId { get; private set; }
    public Guid SourceRevenuId { get; }
    public decimal Montant { get; }

    private RepartitionSourceRevenu()
    {
    }

    public RepartitionSourceRevenu(Guid sourceRevenuId, decimal montant)
    {
        GuardSourceRevenuId(sourceRevenuId);
        GuardMontant(montant);

        Id = Guid.NewGuid();
        SourceRevenuId = sourceRevenuId;
        Montant = montant;
    }

    internal void RattacherATransaction(Guid transactionId) => TransactionId = transactionId;

    private static void GuardSourceRevenuId(Guid sourceRevenuId)
    {
        if (sourceRevenuId == Guid.Empty)
            throw new ArgumentException("Une répartition doit être rattachée à une source de revenu.", nameof(sourceRevenuId));
    }

    private static void GuardMontant(decimal montant)
    {
        if (montant <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(montant),
                montant,
                "Le montant d'une répartition doit être strictement positif.");
    }
}
