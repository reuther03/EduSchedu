using System.Text.RegularExpressions;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

public sealed partial record Email : ValueObject
{
    public string Value { get; }

    private Email()
    {
    }

    public Email(string value)
    {
        Validate(value);
        Value = value;
    }

    private static void Validate(string value)
    {
        if (!EmailRegex().IsMatch(value))
            throw new DomainException($"Invalid email address: {value}");
    }

    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string email) => new(email);


    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-z]{2,}$")]
    private static partial Regex EmailRegex();
}