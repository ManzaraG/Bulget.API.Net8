using Budget.API.Policies;
using Budget.Application.Dtos.Bilans;
using Budget.Application.Features.Bilans.Queries;
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
}
