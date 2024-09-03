using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Modules.Schools.Domain.Schools.Ids;

public record LanguageProficiencyId : EntityId
{
    public LanguageProficiencyId(Guid value) : base(value)
    {
    }

    public static LanguageProficiencyId New() => new(Guid.NewGuid());
    public static LanguageProficiencyId From(Guid value) => new(value);
    public static LanguageProficiencyId From(string value) => new(Guid.Parse(value));

    public static implicit operator Guid(LanguageProficiencyId userId) => userId.Value;
    public static implicit operator LanguageProficiencyId(Guid userId) => new(userId);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}