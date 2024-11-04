using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users.Students;

public record Grade : ValueObject
{
    public float GradeValue { get; }
    public int? Percentage { get; }
    public string? Description { get; }
    public GradeType GradeType { get; }
    public int? Weight { get; }
    public UserId AssignedBy { get; }
    public DateOnly AssignedAt { get; }

    private Grade()
    {
    }

    public Grade(float gradeValue, int? percentage, string? description, GradeType gradeType, int? weight, UserId assignedBy)
    {
        GradeValue = gradeValue;
        Percentage = ValidatePercentage(percentage) ? percentage : throw new DomainException("Percentage must be between 0 and 100");
        Description = description;
        GradeType = gradeType;
        Weight = ValidateWeight(weight) ? weight : throw new DomainException("Weight must be between 1 and 10");
        AssignedBy = assignedBy;
        AssignedAt = DateOnly.FromDateTime(DateTime.Now);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return GradeValue;
        yield return Percentage ?? 0;
        yield return Description ?? string.Empty;
        yield return GradeType;
        yield return Weight ?? 0;
        yield return AssignedBy;
        yield return AssignedAt;
    }

    private static bool ValidatePercentage(int? percentage)
    {
        if (percentage is null)
            return true;

        return percentage is >= 0 and <= 100;
    }

    private static bool ValidateWeight(int? weight)
    {
        if (weight is null)
            return true;

        return weight is >= 1 and <= 10;
    }
}