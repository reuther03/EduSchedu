using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class Class : Entity<ClassId>
{
    private readonly List<Student> _students = [];

    public IReadOnlyList<Student> Students => _students.AsReadOnly();
}