using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EduSchedu.Shared.Abstractions.Auth;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EduSchedu.Shared.Infrastructure.Auth;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(string userId, string email, Role role)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.UniqueName, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimConsts.Role, role.ToString())
        };

        var now = DateTime.UtcNow;

        var polandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

        var localNow = TimeZoneInfo.ConvertTimeFromUtc(now, polandTimeZone);

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256Signature
        );

        var expires = localNow.Add(_jwtOptions.Expiry);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            expires,
            signingCredentials
        );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}

public static class ClaimConsts
{
    public const string UserId = "userId";
    public const string Email = "email";
    public const string Role = "role";
}