using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class Class : Entity<ClassId>
{
    public Name Name { get; private set; }

    private Class()
    {
    }

    private Class(ClassId id, Name name) : base(id)
    {
        Name = name;
    }

    public static Class Create(Name name)
        => new Class(ClassId.New(), name);
}