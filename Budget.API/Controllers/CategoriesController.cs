using Budget.API.Policies;
using Budget.Application.Dtos.Categories;
using Budget.Application.Dtos.Common;
using Budget.Application.Features.Categories.Commands;
using Budget.Application.Features.Categories.Queries;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers;

[ApiController]
[Authorize(Policy = AuthorizationPolicyNames.RequireAuthenticatedUser)]
[Route("api/categories")]
public sealed class CategoriesController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CategorieDto>> Create(CreateCategorieDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateCategorieCommand(dto.Nom, dto.Type), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CategorieDto>> Update(Guid id, UpdateCategorieDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateCategorieCommand(id, dto.Nom), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteCategorieCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategorieDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCategorieByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<CategorieDto>>> GetList(
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
