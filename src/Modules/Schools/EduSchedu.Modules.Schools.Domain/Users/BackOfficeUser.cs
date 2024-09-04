using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class BackOfficeUser : SchoolUser
{
    private BackOfficeUser(UserId id, Email email, Name fullName, Role role)
        : base(id, email, fullName, role)
    {
    }

    public static BackOfficeUser Create(UserId id, Email email, Name fullName)
        => new BackOfficeUser(id, email, fullName, Role.BackOffice);
}