using System.Security.Claims;
using System.Text.Encodings.Web;
using Inexa.ApiKeys.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Budget.API.AuthenticationServices;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    // Nom du scheme, centralisé ici pour éviter la faute de frappe silencieuse entre
    // l'enregistrement (Program.cs) et les policies qui le référencent (RequireAuthenticatedUser
    // ne suffit pas à repérer une divergence : voir GUIDE-UTILISATEUR.md, section 5).
    public const string SchemeName = "ApiKey";

    // En-têtes HTTP standards à ignorer lors de la recherche d'un header de clé API
    // parmi les en-têtes de la requête. Liste technique, pas un nom de header figé.
    private static readonly HashSet<string> StandardHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Accept", "Accept-Encoding", "Accept-Language", "Authorization", "Cache-Control",
        "Connection", "Content-Length", "Content-Type", "Cookie", "Host", "User-Agent"
    };

    private readonly IApiKeyRepository _repository;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IApiKeyRepository repository)
        : base(options, logger, encoder)
    {
        _repository = repository;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // On ne connaît, ni ne fige, aucun nom de header à l'avance : on part des
        // en-têtes réellement présents sur la requête et on interroge le dépôt, de façon
        // ciblée, pour chacun d'entre eux. Une requête légitime n'en porte qu'un seul.
        var candidateHeaderNames = Request.Headers.Keys.Where(k => !StandardHeaders.Contains(k));

        foreach (var headerName in candidateHeaderNames)
        {
            if (!Request.Headers.TryGetValue(headerName, out var rawKeyHeader)) continue;

            // Requête ciblée en base sur ce header ET ce scheme précis (au lieu de charger
            // toutes les clés actives) : le filtre par Scheme.Name garantit qu'une clé émise
            // pour un autre usage n'est jamais retournée ici, même si son HeaderName
            // correspondait par erreur de configuration à un autre scheme.
            var activeKeysForHeader = await _repository.GetActiveByHeaderNameAndSchemeAsync(headerName, Scheme.Name);

            foreach (var candidate in activeKeysForHeader)
            {
                var computedHash = ApiKeyHasher.ComputeHmacSha256(rawKeyHeader.ToString(), candidate.Pepper);
                if (!ApiKeyHasher.FixedTimeEquals(candidate.KeyHash, computedHash)) continue;
                if (!candidate.IsActiveAt(DateTimeOffset.UtcNow)) continue;

                // Traduit chaque scope (Feature/Action) en claim, pour que vos policies
                // d'autorisation puissent utiliser RequireClaim exactement comme avec un
                // ClaimsPrincipal JWT classique.
                var scopes = await _repository.GetScopesAsync(candidate.Id);
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, candidate.Id.ToString()),
                    new(ClaimTypes.Name, candidate.NomUtilisateur),
                    new(ClaimTypes.GivenName, candidate.NomUtilisateur)
                };
                claims.AddRange(scopes.Select(s => new Claim(s.Feature, s.Action)));

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
        }

        return AuthenticateResult.NoResult();
    }
}
