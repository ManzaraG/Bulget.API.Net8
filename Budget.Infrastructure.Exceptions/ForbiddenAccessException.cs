namespace Budget.Infrastructure.Exceptions;

public sealed class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message)
        : base(message)
    {
    }
}
