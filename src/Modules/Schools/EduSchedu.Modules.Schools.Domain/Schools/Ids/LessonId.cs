using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Modules.Schools.Domain.Schools.Ids;

public record LessonId : EntityId
{
    public LessonId(Guid value) : base(value)
    {
    }

    public static LessonId New() => new(Guid.NewGuid());
    public static LessonId From(Guid value) => new(value);
    public static LessonId From(string value) => new(Guid.Parse(value));

    public static implicit operator Guid(LessonId userId) => userId.Value;
    public static implicit operator LessonId(Guid userId) => new(userId);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}