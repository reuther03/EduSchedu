using EduSchedu.Modules.Users.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Users.Application.Users.Dtos;

public class UserDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public Role Role { get; init; }
    public bool IsPasswordChanged { get; init; }

    public static UserDto AsDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            IsPasswordChanged = user.IsPasswordChanged
        };
    }
}