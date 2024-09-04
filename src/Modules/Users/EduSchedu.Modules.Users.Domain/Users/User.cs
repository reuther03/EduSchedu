using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Users.Domain.Users;

public class User : AggregateRoot<UserId>
{
    public Email Email { get; private set; }
    public Name FullName { get; private set; }
    public Password Password { get; private set; }
    public Role Role { get; private set; }
    public bool IsPasswordChanged { get; private set; }

    protected User()
    {
    }

    protected User(UserId id, Email email, Name fullName, Password password, Role role, bool isPasswordChanged) : base(id)
    {
        Email = email;
        FullName = fullName;
        Password = password;
        Role = role;
        IsPasswordChanged = isPasswordChanged;
    }

    public static User Create(Email email, Name fullName, Password password, Role role)
        => new User(UserId.New(), email, fullName, password, role, false);

    public void ChangePassword(Password newPassword)
    {
        Password = Password.Create(newPassword);
        IsPasswordChanged = true;
    }

    private void ChangePasswordStatus()
        => IsPasswordChanged = true;
}