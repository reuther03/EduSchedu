using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Student : SchoolUser
{
    public ClassId ClassId { get; private set; }

    private Student()
    {
    }

    private Student(UserId id, Name fullName, Email email, ClassId classId)
        : base(id, email, fullName)
    {
        ClassId = classId;
    }

    public static Student Create(UserId id, Name fullName, Email email, ClassId classId)
        => new Student(id, fullName, email, classId);
}