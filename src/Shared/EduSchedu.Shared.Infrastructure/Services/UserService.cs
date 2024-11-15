using System.Security.Claims;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.AspNetCore.Http;

namespace EduSchedu.Shared.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    public UserId? UserId => IsAuthenticated ? GetUserIdFromClaims(_httpContextAccessor.HttpContext?.User) : null;
    public Abstractions.Kernel.ValueObjects.Email? Email => IsAuthenticated ? GetEmailFromClaims(_httpContextAccessor.HttpContext?.User) : null;
    public Role? Role => IsAuthenticated ? GetRoleFromClaims(_httpContextAccessor.HttpContext?.User) : null;

    private static UserId? GetUserIdFromClaims(ClaimsPrincipal? claims)
    {
        if (claims is null)
            return null;

        var userId = claims.FindFirst(ClaimTypes.Name)?.Value;
        return userId is null ? null : UserId.From(userId);
    }

    private static Abstractions.Kernel.ValueObjects.Email? GetEmailFromClaims(ClaimsPrincipal? claims)
    {
        if (claims is null)
            return null;

        var email = claims.FindFirst(ClaimTypes.Email)?.Value;
        return email is null ? null : new Abstractions.Kernel.ValueObjects.Email(email);
    }

    private static Role? GetRoleFromClaims(ClaimsPrincipal? claims)
    {
        if (claims is null)
            return null;

        var role = claims.FindFirst(ClaimTypes.Role)?.Value;
        return role is null ? null : Enum.Parse<Role>(role);
    }
}