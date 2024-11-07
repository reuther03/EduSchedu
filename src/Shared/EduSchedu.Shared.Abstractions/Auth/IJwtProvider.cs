using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Shared.Abstractions.Auth;

public interface IJwtProvider
{
    public string GenerateToken(string userId, string fullname, string email, Role role);
}