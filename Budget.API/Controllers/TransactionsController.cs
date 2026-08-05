using Budget.API.Policies;
using Budget.Application.Dtos.Transactions;
using Budget.Application.Features.Transactions.Commands;
using Budget.Application.Features.Transactions.Queries;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers;

[ApiController]
[Authorize(Policy = AuthorizationPolicyNames.RequireAuthenticatedUser)]
[Route("api/transactions")]
public sealed class TransactionsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TransactionDto>> Create(CreateTransactionDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateTransactionCommand(dto.Montant, dto.Type, dto.CompteId, dto.CategorieId, dto.Description, dto.Date);
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TransactionDto>> Update(Guid id, UpdateTransactionDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateTransactionCommand(id, dto.Montant, dto.Description, dto.CategorieId, dto.Date);
        var result = await sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteTransactionCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTransactionByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TransactionDto>>> GetList([FromQuery] Guid compteId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTransactionsListQuery(compteId), cancellationToken);
        return Ok(result);
    }
}
