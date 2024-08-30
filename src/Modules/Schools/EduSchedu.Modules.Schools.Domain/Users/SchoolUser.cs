using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public abstract class SchoolUser : Entity<UserId>
{
    public Email Email { get; private set; }
    public Name FullName { get; private set; }

    protected SchoolUser()
    {
    }

    protected SchoolUser(UserId id, Email email, Name fullName)
        : base(id)
    {
        Email = email;
        FullName = fullName;
    }
}