using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Principal : SchoolUser
{

    private Principal()
    {
    }

    private Principal(UserId id, Email email, Name fullName, Role role)
        : base(id, email, fullName, role)
    {
    }

    public static Principal Create(UserId id, Email email, Name fullName)
    {
        var principal = new Principal(id, email, fullName, Role.Principal);
        return principal;
    }

}