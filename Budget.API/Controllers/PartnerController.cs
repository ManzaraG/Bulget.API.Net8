using Budget.API.Policies;
using Budget.Application.Dtos.Bilans;
using Budget.Application.Dtos.Categories;
using Budget.Application.Dtos.Common;
using Budget.Application.Dtos.Transactions;
using Budget.Application.Features.Bilans.Queries;
using Budget.Application.Features.Categories.Queries;
using Budget.Application.Features.Transactions.Queries;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers;

/// <summary>
/// Endpoints dédiés aux intégrations externes authentifiées par clé API (Inexa.ApiKeys),
/// distincts des endpoints "utilisateur" (api/transactions, api/dashboard) qui restent
/// scopés par JWT via ICurrentUserService. Une clé API n'ayant pas d'identité utilisateur,
/// l'appelant doit ici fournir explicitement l'id ciblé (sourceRevenuId/utilisateurId) :
/// aucune vérification de propriété n'est faite, seule la présence du scope requis compte.
/// </summary>
[ApiController]
[Route("api/partner")]
public sealed class PartnerController(ISender sender) : ControllerBase
{
    [Authorize(Policy = AuthorizationPolicyNames.TransactionsReadPolicy)]
    [HttpGet("transactions")]
    public async Task<ActionResult<PagedResultDto<TransactionDto>>> GetTransactions(
        [FromQuery] Guid sourceRevenuId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var result = await sender.Send(new GetTransactionsListForPartnerQuery(sourceRevenuId, page, pageSize), cancellationToken);
        return Ok(result);
    }

    [Authorize(Policy = AuthorizationPolicyNames.DashboardReadPolicy)]
    [HttpGet("dashboard/monthly")]
    public async Task<ActionResult<BilanMensuelDto>> GetDashboardMonthly(
        [FromQuery] Guid utilisateurId,
        [FromQuery] int annee,
        [FromQuery] int mois,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetBilanMensuelForPartnerQuery(utilisateurId, annee, mois), cancellationToken);
        return Ok(result);
    }

    [Authorize(Policy = AuthorizationPolicyNames.CategoriesReadPolicy)]
    [HttpGet("categories")]
    public async Task<ActionResult<PagedResultDto<CategorieDto>>> GetCategories(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var result = await sender.Send(new GetCategoriesListQuery(page, pageSize), cancellationToken);
        return Ok(result);
    }
}
