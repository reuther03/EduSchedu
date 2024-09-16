using EduSchedu.Modules.Schools.Domain.Schools;

namespace EduSchedu.Modules.Schools.Application.Features.Dtos;

public class ClassDto
{
    public string Name { get; init; } = null!;
    public LanguageProficiency? LanguageProficiency { get; set; }
    public List<LessonDto> Lessons { get; set; } = [];

    public static ClassDto AsDto(Class @class)
    {
        return new ClassDto
        {
            Name = @class.Name.Value,
            LanguageProficiency = @class.LanguageProficiency,
            Lessons = @class.Lessons.Select(LessonDto.AsDto).ToList()
        };
    }
}