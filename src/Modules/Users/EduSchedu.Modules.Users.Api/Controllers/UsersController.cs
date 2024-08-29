using EduSchedu.Modules.Users.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduSchedu.Modules.Users.Api.Controllers;

internal class UsersController : BaseController
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(SignUpCommand request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }
}