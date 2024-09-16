using EduSchedu.Modules.Schools.Domain.Schools;

namespace EduSchedu.Modules.Schools.Application.Features.Dtos;

public class SchoolDto
{
    public Guid SchoolId { get; init; }
    public string City { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public string Email { get; init; } = null!;

    public static SchoolDto AsDto(School school)
    {
        return new SchoolDto
        {
            SchoolId = school.Id,
            City = school.Address.City,
            PhoneNumber = school.PhoneNumber,
            Email = school.Email.Value
        };
    }
}