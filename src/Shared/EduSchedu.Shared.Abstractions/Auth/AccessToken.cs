using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Shared.Abstractions.Auth;

public class AccessToken
{
    public string Token { get; init; } = null!;
    public Guid UserId { get; init; }
    public string FullName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public Role Role { get; init; }


    public static AccessToken Create(string token, Guid userId, string fullName, string email, Role role)
    {
        return new AccessToken
        {
            Token = token,
            UserId = userId,
            FullName = fullName,
            Email = email,
            Role = role
        };
    }
}