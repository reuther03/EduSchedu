using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Modules.Schools.Domain.Users.Students;

public record Grade : ValueObject
{
    public float GradeValue { get; }
    //todo: walidacja czy percentage jest w zakresie 0-100/ czy nie powinno byc stringiem lub string zapisywany w bazie
    public int? Percentage { get; }
    public string? Description { get; }
    public DateTime CreatedAt { get; private set; }

    private Grade()
    {
    }

    public Grade(float gradeValue, int? percentage, string? description)
    {
        GradeValue = gradeValue;
        Percentage = percentage;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return GradeValue;
        yield return Percentage ?? 0;
        yield return Description ?? string.Empty;
    }
}