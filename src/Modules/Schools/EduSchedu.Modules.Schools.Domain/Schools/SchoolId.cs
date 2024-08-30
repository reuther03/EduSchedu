using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public record SchoolId : EntityId
{
    public SchoolId(Guid value) : base(value)
    {
    }

    public static SchoolId New() => new(Guid.NewGuid());
    public static SchoolId From(Guid value) => new(value);
    public static SchoolId From(string value) => new(Guid.Parse(value));

    public static implicit operator Guid(SchoolId userId) => userId.Value;
    public static implicit operator SchoolId(Guid userId) => new(userId);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}