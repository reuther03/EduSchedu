using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Modules.Schools.Domain.Users;

public record IndexNumber : ValueObject
{
    private static readonly Dictionary<string, int> SchoolCounters = new();
    public string Value { get; private set; }

    private IndexNumber()
    {
    }

    private IndexNumber(string schoolName)
    {
        Value = Generate(schoolName);
    }

    public static IndexNumber Create(string schoolName)
    {
        if (string.IsNullOrWhiteSpace(schoolName))
        {
            throw new DomainException("School name cannot be empty", nameof(schoolName));
        }

        return new IndexNumber(schoolName);
    }

    private string Generate(string schoolName)
    {
        SchoolCounters.TryAdd(schoolName, 1);

        Value = $"{schoolName}_{SchoolCounters[schoolName]++}";
        return Value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}