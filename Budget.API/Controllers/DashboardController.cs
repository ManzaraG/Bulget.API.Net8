using Budget.API.Policies;
using Budget.Application.Dtos.Bilans;
using Budget.Application.Dtos.Dashboard;
using Budget.Application.Features.Bilans.Queries;
using Budget.Application.Features.Dashboard.Queries;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers;

[ApiController]
[Authorize(Policy = AuthorizationPolicyNames.RequireAuthenticatedUser)]
[Route("api/dashboard")]
public sealed class DashboardController(ISender sender) : ControllerBase
{
    [HttpGet("monthly")]
    public async Task<ActionResult<BilanMensuelDto>> GetMonthly([FromQuery] int annee, [FromQuery] int mois, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetBilanMensuelQuery(annee, mois), cancellationToken);
        return Ok(result);
    }

    [HttpGet("trend")]
    public async Task<ActionResult<IReadOnlyList<DashboardTrendPointDto>>> GetTrend(
        [FromQuery] int annee,
        [FromQuery] int mois,
        [FromQuery] int nombreMois = 6,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetDashboardTrendQuery(annee, mois, nombreMois), cancellationToken);
        return Ok(result);
    }

    [HttpGet("accounts")]
    public async Task<ActionResult<IReadOnlyList<SourceRevenuBudgetDto>>> GetAccounts(
        [FromQuery] int annee,
        [FromQuery] int mois,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetDashboardComptesQuery(annee, mois), cancellationToken);
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<CategorieBudgetDto>>> GetCategories(
        [FromQuery] int annee,
        [FromQuery] int mois,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetDashboardCategoriesQuery(annee, mois), cancellationToken);
        return Ok(result);
    }
}
