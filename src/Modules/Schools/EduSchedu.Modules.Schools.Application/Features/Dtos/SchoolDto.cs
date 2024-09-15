using EduSchedu.Modules.Schools.Domain.Schools;

namespace EduSchedu.Modules.Schools.Application.Features.Dtos;

public class SchoolDto
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string? MapCoordinates { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Guid HeadmasterId { get; set; }
    public List<Guid> TeacherIds { get; set; }
    public List<ClassDto> Classes { get; set; }

    public static SchoolDto AsDto(School school)
    {
        return new SchoolDto
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
            Classes = school.Classes.Select(ClassDto.AsDto).ToList()
        };
    }
}