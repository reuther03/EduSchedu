using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public abstract class SchoolUser : AggregateRoot<UserId>
{
    public Email Email { get; private set; }
    public Name FullName { get; private set; }
    public Role Role { get; private set; }

    protected SchoolUser()
    {
    }

    protected SchoolUser(UserId id, Email email, Name fullName, Role role)
        : base(id)
    {
        Email = email;
        FullName = fullName;
        Role = role;
    }
}