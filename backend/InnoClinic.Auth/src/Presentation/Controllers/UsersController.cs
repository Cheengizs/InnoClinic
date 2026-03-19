using Application.Features.Accounts.Queries.GetAdminsEmails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getAllAdmins")]
    public async Task<IActionResult> GetAllAdmins()
    {
        var request = new GetAdminEmailsQuery();
        var some = await _mediator.Send(request);

        return Ok(some.Value);
    }
}
