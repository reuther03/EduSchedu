using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

public record Description : ValueObject
{
    public string Value { get; }

    public Description(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Description cannot be empty.");
        }

        Value = value;
    }

    public static implicit operator string(Description description) => description.Value;
    public static implicit operator Description(string description) => new(description);

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}