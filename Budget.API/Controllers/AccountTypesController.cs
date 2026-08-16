using Budget.API.Policies;
using Budget.Application.Dtos.Common;
using Budget.Application.Dtos.TypesSourceRevenu;
using Budget.Application.Features.TypesSourceRevenu.Commands;
using Budget.Application.Features.TypesSourceRevenu.Queries;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers;

[ApiController]
[Authorize(Policy = AuthorizationPolicyNames.RequireAuthenticatedUser)]
[Route("api/account-types")]
public sealed class AccountTypesController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TypeSourceRevenuDto>> Create(CreateTypeSourceRevenuDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateTypeSourceRevenuCommand(dto.Nom), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TypeSourceRevenuDto>> Update(Guid id, UpdateTypeSourceRevenuDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateTypeSourceRevenuCommand(id, dto.Nom), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteTypeSourceRevenuCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TypeSourceRevenuDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTypeSourceRevenuByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<TypeSourceRevenuDto>>> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var result = await sender.Send(new GetTypesSourceRevenuListQuery(page, pageSize), cancellationToken);
        return Ok(result);
    }
}
