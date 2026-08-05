using Budget.API.Policies;
using Budget.Application.Dtos.Comptes;
using Budget.Application.Features.Comptes.Commands;
using Budget.Application.Features.Comptes.Queries;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers;

[ApiController]
[Authorize(Policy = AuthorizationPolicyNames.RequireAuthenticatedUser)]
[Route("api/accounts")]
public sealed class AccountsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CompteDto>> Create(CreateCompteDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateCompteCommand(dto.Nom), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CompteDto>> Update(Guid id, UpdateCompteDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateCompteCommand(id, dto.Nom), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteCompteCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CompteDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCompteByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CompteDto>>> GetList(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetComptesListQuery(), cancellationToken);
        return Ok(result);
    }
}
