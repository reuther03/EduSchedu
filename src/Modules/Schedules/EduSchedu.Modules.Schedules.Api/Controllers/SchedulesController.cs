using EduSchedu.Modules.Schedules.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduSchedu.Modules.Schedules.Api.Controllers;

internal class SchedulesController  : BaseController
{
    private readonly ISender _sender;

    public SchedulesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserScheduleItems([FromQuery] GetUserScheduleItems query)
    {
        var result = await _sender.Send(query);
        return Ok(result);
    }
}