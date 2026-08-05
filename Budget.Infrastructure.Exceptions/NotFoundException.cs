namespace Budget.Infrastructure.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"« {name} » ({key}) est introuvable.")
    {
    }
}
