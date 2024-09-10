using EduSchedu.Modules.Schools.Application.Features.Commands.Class;
using EduSchedu.Modules.Schools.Application.Features.Commands.Schools;
using EduSchedu.Modules.Schools.Application.Features.Commands.User;
using EduSchedu.Shared.Abstractions.Kernel.Attribute;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduSchedu.Modules.Schools.Api.Controllers;

internal class SchoolsController : BaseController
{
    private readonly ISender _sender;

    public SchoolsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("create")]
    [AuthorizeRoles(Role.HeadMaster)]
    public async Task<IActionResult> CreateSchool([FromBody] CreateSchoolCommand command)
    {
        var result = await _sender.Send(command);
        return Ok(result);
    }

    [HttpPost("{schoolId:guid}/teacher/add-language-proficiency")]
    [AuthorizeRoles(Role.HeadMaster, Role.BackOffice)]
    public async Task<IActionResult> AddLanguageProficiency([FromBody] AddLanguageProficiencyCommand command, [FromRoute] Guid schoolId)
    {
        var result = await _sender.Send(command with { SchoolId = schoolId });
        return Ok(result);
    }

    [HttpPost("{schoolId:guid}/class/create")]
    [AuthorizeRoles(Role.HeadMaster, Role.BackOffice)]
    public async Task<IActionResult> CreateClass([FromBody] CreateClassCommand command, [FromRoute] Guid schoolId)
    {
        var result = await _sender.Send(command with { SchoolId = schoolId });
        return Ok(result);
    }
}