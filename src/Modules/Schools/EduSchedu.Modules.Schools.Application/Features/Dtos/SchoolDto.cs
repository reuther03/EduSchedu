using EduSchedu.Modules.Schools.Domain.Schools;

namespace EduSchedu.Modules.Schools.Application.Features.Dtos;

public class SchoolDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string City { get; init; } = null!;
    public string Email { get; init; } = null!;

    public static SchoolDto AsDto(School school)
    {
        return new SchoolDto
        {
            Id = school.Id,
            Name = school.Name.Value,
            City = school.Address.City,
            Email = school.Email.Value
        };
    }
}