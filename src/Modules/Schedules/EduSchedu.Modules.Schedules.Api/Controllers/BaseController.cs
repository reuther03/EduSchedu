using Microsoft.AspNetCore.Mvc;

namespace EduSchedu.Modules.Schedules.Api.Controllers;

[ApiController]
[Route(SchedulesModule.BasePath + "/[controller]")]
internal abstract class BaseController : ControllerBase
{
}