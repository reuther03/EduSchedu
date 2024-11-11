﻿using EduSchedu.Modules.Schools.Application.Features.Queries.Users;
using EduSchedu.Shared.Abstractions.Kernel.Attribute;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduSchedu.Modules.Schools.Api.Controllers;

internal class TeacherController : BaseController
{
    private readonly ISender _sender;

    public TeacherController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("/teacher/schedule")]
    [AuthorizeRoles(Role.HeadMaster, Role.BackOffice, Role.Teacher)]
    public async Task<IActionResult> GetTeachersSchedule()
    {
        var result = await _sender.Send(new GetTeachersScheduleQuery());
        return Ok(result);
    }
}