using Microsoft.AspNetCore.Mvc;

namespace EduSchedu.Modules.Schedules.Api.Controllers;

internal class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => Ok("Schedule API");
}