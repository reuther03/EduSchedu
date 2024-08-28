using Microsoft.AspNetCore.Mvc;

namespace EduSchedu.Modules.Users.Api.Controllers;

internal class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "User API";
}