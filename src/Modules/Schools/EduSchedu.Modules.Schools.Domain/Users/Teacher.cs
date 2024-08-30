using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Teacher : SchoolUser
{
    public Role Role { get; private set; }
    public List<Skill> Skills { get; private set; } = [];

    private Teacher()
    {
    }

    private Teacher(UserId id, Name fullName, Email email, Role role, List<Skill> skills)
        : base(id, email, fullName)
    {
        Role = role;
        Skills = skills;
    }

    public static Teacher Create(UserId id, Name fullName, Email email, Role role, List<Skill> skills)
        => new Teacher(id, fullName, email, role, skills);
}