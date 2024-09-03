using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

public record Address : ValueObject
{
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string? MapCoordinates { get; set; }

    public Address(string city, string street, string zipCode, string? mapCoordinates)
    {
        if (
            string.IsNullOrWhiteSpace(city) ||
            string.IsNullOrWhiteSpace(street) ||
            string.IsNullOrWhiteSpace(zipCode)
        )
        {
            throw new ArgumentException("Address cannot be empty");
        }

        City = city;
        Street = street;
        ZipCode = zipCode;
        MapCoordinates = mapCoordinates;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return City;
        yield return Street;
        yield return ZipCode;
        yield return MapCoordinates ?? string.Empty;
    }
}