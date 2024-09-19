using EduSchedu.Modules.Schools.Application.Features.Commands.Class;
using EduSchedu.Modules.Schools.Application.Features.Commands.Schools;
using EduSchedu.Modules.Schools.Application.Features.Commands.User;
using EduSchedu.Modules.Schools.Application.Features.Queries.School;
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

    [HttpGet]
    [AuthorizeRoles(Role.HeadMaster, Role.BackOffice, Role.Teacher)]
    public async Task<IActionResult> GetUserSchools([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _sender.Send(new GetUserSchoolsQuery(page, pageSize));
        return Ok(result);
    }

    [HttpGet("{schoolId:guid}")]
    [AuthorizeRoles(Role.HeadMaster, Role.BackOffice, Role.Teacher)]
    public async Task<IActionResult> GetSchool([FromRoute] Guid schoolId)
    {
        var result = await _sender.Send(new GetSchoolCommand(schoolId));
        return Ok(result);
    }


    [HttpPost]
    [AuthorizeRoles(Role.HeadMaster)]
    public async Task<IActionResult> CreateSchool([FromBody] CreateSchoolCommand command)
    {
        var result = await _sender.Send(command);
        return Ok(result);
    }

    [HttpPost("{schoolId:guid}/teacher")]
    [AuthorizeRoles(Role.HeadMaster)]
    public async Task<IActionResult> AddTeacher([FromBody] AddExistingUserCommand command, [FromRoute] Guid schoolId)
    {
        var result = await _sender.Send(command with { SchoolId = schoolId });
        return Ok(result);
    }

    [HttpPost("{schoolId:guid}/teacher/add-language-proficiency")]
    [AuthorizeRoles(Role.HeadMaster, Role.BackOffice)]
    public async Task<IActionResult> AddLanguageProficiency([FromBody] AddTeacherLanguageProficiencyCommand command, [FromRoute] Guid schoolId)
    {
        var result = await _sender.Send(command with { SchoolId = schoolId });
        return Ok(result);
    }

    [HttpPost("{schoolId:guid}/class")]
    [AuthorizeRoles(Role.HeadMaster, Role.BackOffice)]
    public async Task<IActionResult> CreateClass([FromBody] CreateClassCommand command, [FromRoute] Guid schoolId)
    {
        var result = await _sender.Send(command with { SchoolId = schoolId });
        return Ok(result);
    }

    [HttpPost("{schoolId:guid}/class/{classId:guid}/lesson")]
    [AuthorizeRoles(Role.HeadMaster, Role.BackOffice)]
    public async Task<IActionResult> AddClassLesson([FromBody] AddClassLessonCommand command, [FromRoute] Guid schoolId, [FromRoute] Guid classId)
    {
        var result = await _sender.Send(command with { SchoolId = schoolId, ClassId = classId });
        return Ok(result);
    }

    [HttpPost("{schoolId:guid}/class/{classId:guid}/language-proficiency")]
    [AuthorizeRoles(Role.HeadMaster, Role.BackOffice)]
    public async Task<IActionResult> AddClassLanguageProficiency([FromBody] AddClassLanguageProficiencyCommand command, [FromRoute] Guid schoolId,
        [FromRoute] Guid classId)
    {
        var result = await _sender.Send(command with { SchoolId = schoolId, ClassId = classId });
        return Ok(result);
    }

    [HttpPost("{schoolId:guid}/teacher/{teacherId:guid}/schedule/lesson")]
    [AuthorizeRoles(Role.HeadMaster, Role.BackOffice)]
    public async Task<IActionResult> AddScheduleLesson([FromBody] AddScheduleLessonCommand command, [FromRoute] Guid schoolId, [FromRoute] Guid teacherId)
    {
        var result = await _sender.Send(command with { SchoolId = schoolId, UserId = teacherId });
        return Ok(result);
    }

    [HttpPost("{schoolId:guid}/assign-teachers")]
    [AuthorizeRoles(Role.HeadMaster)]
    public async Task<IActionResult> AssignTeachersToLessons([FromRoute] Guid schoolId)
    {
        var result = await _sender.Send(new AssignTeacherToLessonCommand(SchoolId: schoolId));
        return Ok(result);
    }

    [HttpPost("{schoolId:guid}/teacher/{teacherId:guid}/schedule")]
    [AuthorizeRoles(Role.HeadMaster, Role.BackOffice)]
    public async Task<IActionResult> AddScheduleItem([FromBody] AddScheduleItemCommand command, [FromRoute] Guid schoolId, [FromRoute] Guid teacherId)
    {
        var result = await _sender.Send(command with { SchoolId = schoolId, UserId = teacherId });
        return Ok(result);
    }
}