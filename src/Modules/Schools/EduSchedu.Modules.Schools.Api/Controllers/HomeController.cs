using Microsoft.AspNetCore.Mvc;

namespace EduSchedu.Modules.Schools.Api.Controllers;

internal class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "SchoolUser API";
}