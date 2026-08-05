namespace Budget.Application.Adapters.Tokens;

public sealed class JwtSettings
{
    public required string SecretKey { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public int ExpirationMinutes { get; init; } = 60;
}
