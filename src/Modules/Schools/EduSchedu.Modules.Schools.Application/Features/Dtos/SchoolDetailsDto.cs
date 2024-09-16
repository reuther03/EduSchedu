using EduSchedu.Modules.Schools.Domain.Schools;

namespace EduSchedu.Modules.Schools.Application.Features.Dtos;

public class SchoolDetailsDto
{
    public string Name { get; init; } = null!;
    public string City { get; init; } = null!;
    public string Street { get; init; } = null!;
    public string ZipCode { get; init; } = null!;
    public string? MapCoordinates { get; init; }
    public string PhoneNumber { get; init; } = null!;
    public string Email { get; init; } = null!;
    public Guid HeadmasterId { get; init; }
    public List<Guid> TeacherIds { get; init; } = [];
    public List<Guid> ClassIds { get; init; } = [];

    public static SchoolDetailsDto AsDto(School school)
    {
        return new SchoolDetailsDto
        {
            Name = school.Name.Value,
            City = school.Address.City,
            Street = school.Address.Street,
            ZipCode = school.Address.ZipCode,
            MapCoordinates = school.Address.MapCoordinates,
            PhoneNumber = school.PhoneNumber,
            Email = school.Email.Value,
            HeadmasterId = school.HeadmasterId,
            TeacherIds = school.TeacherIds.Select(x => x.Value).ToList(),
            ClassIds = school.Classes.Select(x => x.Id.Value).ToList()
        };
    }
}