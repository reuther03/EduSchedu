using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Modules.Schools.Domain.Users.Students;

public record Grade : ValueObject
{
    public float GradeValue { get; }
    public int? Percentage { get; }
    public string? Description { get; }
    public DateOnly CreatedAt { get; }

    private Grade()
    {
    }

    public Grade(float gradeValue, int? percentage, string? description)
    {
        GradeValue = gradeValue;
        Percentage = ValidatePercentage(percentage) ? percentage : throw new DomainException("Percentage must be between 0 and 100");
        Description = description;
        CreatedAt = DateOnly.FromDateTime(DateTime.Now);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return GradeValue;
        yield return Percentage ?? 0;
        yield return Description ?? string.Empty;
        yield return CreatedAt;
    }

    private static bool ValidatePercentage(int? percentage)
        => percentage is >= 0 and <= 100;
}