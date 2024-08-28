using Microsoft.AspNetCore.Mvc;

namespace EduSchedu.Modules.Users.Api.Controllers;

[ApiController]
[Route(UsersModule.BasePath + "/[controller]")]
internal abstract class BaseController : ControllerBase
{
}