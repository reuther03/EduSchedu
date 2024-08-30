using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Teacher : Entity<UserId>
{
    public Email Email { get; private set; }
    public Name FullName { get; private set; }
    public Role Role { get; private set; }
    public SchoolId SchoolId { get; private set; }
    public List<Skill> Skills { get; private set; } = [];

    private Teacher()
    {
    }

    private Teacher(UserId id, Email email, Name fullName, Role role, SchoolId schoolId)
        : base(id)
    {
        Email = email;
        FullName = fullName;
        Role = role;
        SchoolId = schoolId;
    }

    public static Teacher Create(UserId id, Email email, Name fullName, Role role, SchoolId schoolId)
        => new Teacher(id, email, fullName, role, schoolId);
}