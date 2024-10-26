using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Headmaster : Teacher
{
    private Headmaster(UserId id, Email email, Name fullName) : base(id, email, fullName, Role.HeadMaster)
    {
    }

    public static Headmaster Create(UserId id, Email email, Name fullName)
    {
        var headmaster = new Headmaster(id, email, fullName);
        return headmaster;
    }
}