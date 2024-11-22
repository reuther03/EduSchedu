using EduSchedu.Modules.Schedules.Application.Features.Queries;
using EduSchedu.Shared.Abstractions.Kernel.Attribute;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduSchedu.Modules.Schedules.Api.Controllers;

internal class SchedulesController : BaseController
{
    private readonly ISender _sender;

    public SchedulesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserScheduleItems([FromBody] GetUserScheduleItems query)
    {
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [HttpGet("{schoolId:guid}/teachers/schedules")]
    [AuthorizeRoles(Role.HeadMaster)]
    public async Task<IActionResult> GetTeachersSchedules([FromRoute] Guid schoolId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _sender.Send(new GetTeachersSchedulesQuery(schoolId, page, pageSize) { SchoolIdQuery = schoolId });
        return Ok(result);
    }
}