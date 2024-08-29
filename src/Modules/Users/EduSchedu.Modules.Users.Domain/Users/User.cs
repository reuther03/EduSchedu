using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Users.Domain.Users;

public class User : AggregateRoot<UserId>
{
    public Email Email { get; private set; }
    public Name FullName { get; private set; }
    public Password Password { get; private set; }

    protected User()
    {
    }

    protected User(UserId id, Email email, Name fullName, Password password)
        : base(id)
    {
        Email = email;
        FullName = fullName;
        Password = password;
    }

    public static User Create(Email email, Name fullName, Password password)
    {
        return new User(UserId.New(), email, fullName, password);
    }
}