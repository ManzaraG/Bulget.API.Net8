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
    public string GenerateToken(Utilisateur utilisateur)
    {
        var settings = jwtSettings.Value;

        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, utilisateur.Id.ToString()),
            new Claim(ClaimTypes.Email, utilisateur.Email),
            new Claim(ClaimTypes.Name, utilisateur.Nom),
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(settings.ExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
