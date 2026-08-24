using Budget.API.Policies;
using Budget.Application.Dtos.Common;
using Budget.Application.Dtos.SourcesRevenu;
using Budget.Application.Features.SourcesRevenu.Commands;
using Budget.Application.Features.SourcesRevenu.Queries;
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
    public async Task<ActionResult<SourceRevenuDto>> Create(CreateSourceRevenuDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateSourceRevenuCommand(dto.Nom, dto.TypeId, dto.DeviseId), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SourceRevenuDto>> Update(Guid id, UpdateSourceRevenuDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateSourceRevenuCommand(id, dto.Nom, dto.TypeId, dto.DeviseId), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteSourceRevenuCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SourceRevenuDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetSourceRevenuByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<SourceRevenuDto>>> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var result = await sender.Send(new GetSourcesRevenuListQuery(page, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<ActionResult<SourceRevenuDto>> Activate(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ActivateSourceRevenuCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<ActionResult<SourceRevenuDto>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeactivateSourceRevenuCommand(id), cancellationToken);
        return Ok(result);
    }
}
