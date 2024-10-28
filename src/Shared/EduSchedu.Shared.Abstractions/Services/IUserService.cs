using System.Diagnostics.CodeAnalysis;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Shared.Abstractions.Services;

public interface IUserService
{
    [MemberNotNullWhen(true, nameof(UserId),nameof(Email))]
    public bool IsAuthenticated { get; }
    public UserId? UserId { get; }
    public Name FullName { get; }
    public Kernel.ValueObjects.Email? Email { get; }
}