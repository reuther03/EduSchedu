using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Student : SchoolUser
{

    private Student()
    {
    }

    public Student(UserId id, Email email, Name fullName, Role role) : base(id, email, fullName, role)
    {
    }

    public static Student Create(UserId id, Email email, Name fullName)
    {
        return new Student(id, email, fullName, Role.Student);
    }
}