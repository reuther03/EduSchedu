namespace EduSchedu.Shared.Infrastructure.Auth;

public class JwtOptions
{
    internal const string SectionName = "jwt";

    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SecretKey { get; init; }
    public TimeSpan Expiry { get; init; }
}