using Budget.Application.Contrats.Services;
using Microsoft.AspNetCore.Identity;

namespace Budget.Application.Adapters.Passwords;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly Microsoft.AspNetCore.Identity.PasswordHasher<object> _hasher = new();
    private static readonly object HasherUser = new();

    public string Hash(string motDePasse) => _hasher.HashPassword(HasherUser, motDePasse);

    public bool Verify(string motDePasseHash, string motDePasse)
        => _hasher.VerifyHashedPassword(HasherUser, motDePasseHash, motDePasse) != PasswordVerificationResult.Failed;
}
