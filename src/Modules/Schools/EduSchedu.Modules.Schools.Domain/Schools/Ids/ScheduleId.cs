using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Modules.Schools.Domain.Schools.Ids;

public record ScheduleId : EntityId
{
    public ScheduleId(Guid value) : base(value)
    {
    }

    public static ScheduleId New() => new(Guid.NewGuid());
    public static ScheduleId From(Guid value) => new(value);
    public static ScheduleId From(string value) => new(Guid.Parse(value));

    public static implicit operator Guid(ScheduleId userId) => userId.Value;
    public static implicit operator ScheduleId(Guid userId) => new(userId);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}