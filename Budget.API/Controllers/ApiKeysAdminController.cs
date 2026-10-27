using Budget.API.Policies;
using Inexa.ApiKeys.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers;

/// <summary>
/// Administration des clés API (émission, listing, scopes, révocation) directement via l'API,
/// en alternative à l'outil console ApiKeyGenerator.
///
/// Protégé par RequireAuthenticatedUser (JWT) faute de système de rôles dans Budget.API à ce
/// jour : n'importe quel utilisateur connecté peut administrer les clés. Ne jamais protéger ce
/// contrôleur par le scheme ApiKey lui-même — une clé compromise pourrait sinon servir à en
/// créer d'autres. À durcir (rôle admin dédié, liste blanche d'emails...) si l'application
/// accueille un jour plusieurs comptes utilisateurs.
/// </summary>
[ApiController]
[Route("api/admin/api-keys")]
[Authorize(Policy = AuthorizationPolicyNames.RequireAuthenticatedUser)]
public class ApiKeysAdminController : ControllerBase
{
    // IApiKeyIssuer (interface), pas ApiKeyIssuer (type concret) : permet d'injecter ce
    // service par abstraction, comme n'importe quel autre service applicatif — c'est
    // pour cela qu'il a été enregistré ainsi à l'étape 3.
    private readonly IApiKeyIssuer _issuer;
    private readonly IApiKeyRepository _repository;

    public ApiKeysAdminController(IApiKeyIssuer issuer, IApiKeyRepository repository)
    {
        _issuer = issuer;
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<ApiKeyIssuedResult>> Create([FromBody] CreateApiKeyRequest request)
    {
        // Les scopes sont fournis ici, pas ajoutés après coup : IssueAsync les insère avec la
        // clé dans une seule transaction (voir GUIDE-UTILISATEUR.md, sections 3 et 6). Un
        // appel séparé à AddScopeAsync après ce IssueAsync n'offrirait pas cette garantie.
        var result = await _issuer.IssueAsync(
            request.Label, request.HeaderName, request.SchemeName, request.NomUtilisateur,
            request.Email, request.Telephone,
            TimeSpan.FromDays(request.RotationDays),
            scopes: request.Scopes?.Select(s => (s.Feature, s.Action)));

        // ATTENTION : result.RawKey n'est visible qu'ici. Le client (ou l'administrateur qui
        // consulte la réponse) doit la copier immédiatement : elle n'est jamais consultable
        // à nouveau après cette réponse HTTP — exactement comme à la console (tutoriel,
        // GUIDE-UTILISATEUR.md).
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiKeyListPage>> List(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? label = null,
        [FromQuery] ApiKeyStatus? status = null)
    {
        var page = await _repository.ListAsync(new ApiKeyListQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Label = label,
            Status = status
        });
        return Ok(page);
    }

    [HttpGet("{id:guid}/scopes")]
    public async Task<ActionResult<IReadOnlyList<ApiKeyScopeRecord>>> GetScopes(Guid id)
        => Ok(await _repository.GetScopesAsync(id));

    [HttpPost("{id:guid}/scopes")]
    public async Task<IActionResult> AddScope(Guid id, [FromBody] AddScopeRequest request)
    {
        await _repository.AddScopeAsync(id, request.Feature, request.Action);
        return NoContent();
    }

    [HttpPut("{id:guid}/scopes")]
    public async Task<IActionResult> UpdateScopes(Guid id, [FromBody] UpdateScopesRequest request)
    {
        // Ajouts et suppressions en un seul appel — utile pour un changement de périmètre
        // (une clé qui gagne un droit et en perd un autre en même temps).
        await _repository.UpdateScopesAsync(
            id,
            request.ScopesToAdd?.Select(s => (s.Feature, s.Action)),
            request.ScopesToRemove?.Select(s => (s.Feature, s.Action)));
        return NoContent();
    }

    [HttpPost("{id:guid}/revoke")]
    public async Task<IActionResult> Revoke(Guid id)
    {
        await _repository.RevokeAsync(id, updatedBy: User.Identity?.Name);
        return NoContent();
    }
}

public record CreateApiKeyRequest(
    string Label, string HeaderName, string SchemeName, string NomUtilisateur,
    string Email, string Telephone, int RotationDays, List<AddScopeRequest>? Scopes = null);

public record AddScopeRequest(string Feature, string Action);
public record UpdateScopesRequest(List<AddScopeRequest>? ScopesToAdd, List<AddScopeRequest>? ScopesToRemove);
