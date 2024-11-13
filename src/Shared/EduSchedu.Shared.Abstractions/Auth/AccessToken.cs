using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Shared.Abstractions.Auth;

public class AccessToken
{
    public string Token { get; init; } = null!;
    public Guid UserId { get; init; }
    public string Email { get; init; } = null!;
    public Role Role { get; init; }


    public static AccessToken Create(string token, Guid userId, string email, Role role)
    {
        return new AccessToken
        {
            Token = token,
            UserId = userId,
            Email = email,
            Role = role
        };
    }
}