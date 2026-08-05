using Budget.Application.Dtos.Authentication;
using Budget.Application.Features.Authentication.Commands;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public sealed class AuthenticationController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResultDto>> Register(RegisterDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterCommand(dto.Prenom, dto.Nom, dto.Email, dto.MotDePasse, dto.ConfirmationMotDePasse, dto.AccepteConditions),
            cancellationToken);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResultDto>> Login(LoginDto dto, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new LoginCommand(dto.Email, dto.MotDePasse, dto.SeSouvenirDeMoi), cancellationToken);
        return Ok(result);
    }
}
