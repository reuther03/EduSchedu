using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class BackOffice : SchoolUser
{
    private BackOffice(UserId id, Email email, Name fullName, Role role)
        : base(id, email, fullName, role)
    {
    }

    public static BackOffice Create(UserId id, Email email, Name fullName)
    {
        var backOffice = new BackOffice(id, email, fullName, Role.BackOffice);
        return backOffice;
    }
}