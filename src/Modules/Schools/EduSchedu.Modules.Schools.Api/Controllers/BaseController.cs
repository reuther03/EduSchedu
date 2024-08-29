using Microsoft.AspNetCore.Mvc;

namespace EduSchedu.Modules.Schools.Api.Controllers;

[ApiController]
[Route(SchoolsModule.BasePath + "/[controller]")]
internal abstract class BaseController : ControllerBase
{
}