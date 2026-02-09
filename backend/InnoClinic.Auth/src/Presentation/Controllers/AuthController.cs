using Application.Dto.Account;
using Application.Features.Accounts.Commands.Login;
using Application.Features.Accounts.Commands.RefreshTokens;
using Application.Features.Accounts.Commands.RegisterAccount;
using Application.Features.Accounts.Commands.Revoke;
using Application.Features.Accounts.Queries.GetAccountById;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;
using Presentation.Extensions;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;
    
    public AuthController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new RegisterAccountCommand(
            request.Email,
            request.Password,
            request.PhoneNumber,
            AccountRole.Patient 
        );

        var result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails();
        }
    
        return CreatedAtAction(nameof(GetAccountById), new {id = result.Value}, new { id =  result.Value });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(
            request.Email,
            request.Password);
        
        var result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails();
        }

        return Ok(result.Value);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAccountById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAccountByIdQuery(id);
        
        var result = await _sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails();
        }

        return Ok(result.Value);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(request.AccessToken, request.RefreshToken);
        
        var result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails();
        }

        return Ok(result.Value);
    }

    [HttpPost("revoke")]
    [Authorize] 
    public async Task<IActionResult> Revoke([FromBody] RevokeTokenRequest request, CancellationToken cancellationToken)
    {
        var command = new RevokeTokenCommand(request.RefreshToken);
        
        var result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails();
        }

        return Ok(); 
    }
}