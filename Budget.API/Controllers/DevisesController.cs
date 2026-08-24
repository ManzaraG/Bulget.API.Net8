using Budget.API.Policies;
using Budget.Application.Dtos.Common;
using Budget.Application.Dtos.Devises;
using Budget.Application.Features.Devises.Commands;
using Budget.Application.Features.Devises.Queries;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers;

[ApiController]
[Authorize(Policy = AuthorizationPolicyNames.RequireAuthenticatedUser)]
[Route("api/devises")]
public sealed class DevisesController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<DeviseDto>> Create(CreateDeviseDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateDeviseCommand(dto.Nom), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DeviseDto>> Update(Guid id, UpdateDeviseDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateDeviseCommand(id, dto.Nom), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteDeviseCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DeviseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetDeviseByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<DeviseDto>>> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var result = await sender.Send(new GetDevisesListQuery(page, pageSize), cancellationToken);
        return Ok(result);
    }
}
