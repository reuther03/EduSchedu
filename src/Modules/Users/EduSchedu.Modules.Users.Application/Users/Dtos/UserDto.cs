using EduSchedu.Modules.Users.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Users.Application.Users.Dtos;

public class UserDto
{
    public string Email { get; init; } = default!;
    public string FullName { get; init; } = default!;
    public Role Role { get; init; }
    public bool IsPasswordChanged { get; init; }

    public static UserDto AsDto(User user)
        => new UserDto
        {
            Email = user.Email.Value,
            FullName = user.FullName.Value,
            Role = user.Role,
            IsPasswordChanged = user.IsPasswordChanged
        };
}