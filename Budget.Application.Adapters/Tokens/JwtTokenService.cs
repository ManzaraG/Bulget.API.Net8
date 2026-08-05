using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Budget.Application.Contrats.Services;
using Budget.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Budget.Application.Adapters.Tokens;

public sealed class JwtTokenService(IOptions<JwtSettings> jwtSettings) : ITokenService
{
    public TokenResult GenerateToken(Utilisateur utilisateur, bool seSouvenirDeMoi = false)
    {
        var settings = jwtSettings.Value;

        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, utilisateur.Id.ToString()),
            new Claim(ClaimTypes.Email, utilisateur.Email),
            new Claim(ClaimTypes.Name, utilisateur.Prenom),
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = seSouvenirDeMoi
            ? DateTime.UtcNow.AddDays(settings.RememberMeExpirationDays)
            : DateTime.UtcNow.AddMinutes(settings.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new TokenResult(new JwtSecurityTokenHandler().WriteToken(token), expiration);
    }
}
