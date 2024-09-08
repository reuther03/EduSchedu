using EduSchedu.Modules.Users.Application.Users.Commands;
using EduSchedu.Shared.Abstractions.Kernel.Attribute;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
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

    [HttpPost("sign-up/headmaster")]
    public async Task<IActionResult> SignUp([FromBody] SignUpHeadmasterCommand request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("create-teacher")]
    [AuthorizeRoles(Role.HeadMaster)]
    public async Task<IActionResult> CreateTeacher([FromBody] CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }

    // [HttpDelete("delete-teacher")]
    // [AuthorizeRoles(Role.HeadMaster)]
    // public async Task<IActionResult> DeleteTeacher([FromBody] DeleteTeacherCommand request, CancellationToken cancellationToken)
    // {
    //     var result = await _sender.Send(request, cancellationToken);
    //     return Ok(result);
    // }
}