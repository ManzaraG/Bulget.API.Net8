using System.Security.Claims;
using Budget.Application.Contrats.Identities;
using Microsoft.AspNetCore.Http;

namespace Budget.Application.Adapters.Identities;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? UtilisateurId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public string? Email => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

    public bool EstAuthentifie => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
