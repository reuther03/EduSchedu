using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Modules.Schools.Domain.Schools.Ids;

public record ClassId : EntityId
{
    public ClassId(Guid value) : base(value)
    {
    }

    public static ClassId New() => new(Guid.NewGuid());
    public static ClassId From(Guid value) => new(value);
    public static ClassId From(string value) => new(Guid.Parse(value));

    public static implicit operator Guid(ClassId userId) => userId.Value;
    public static implicit operator ClassId(Guid userId) => new(userId);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}