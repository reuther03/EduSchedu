using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Student : SchoolUser
{
    public int Grade { get; private set; }

    private Student()
    {
    }

    public Student(UserId id, Email email, Name fullName, Role role, int grade) : base(id, email, fullName, role)
    {
        Grade = grade;
    }
}