using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Users.Infrastructure;

public class BackOfficeUserOptions
{
    public string Email { get; set; }
    public string Password { get; set; }
}