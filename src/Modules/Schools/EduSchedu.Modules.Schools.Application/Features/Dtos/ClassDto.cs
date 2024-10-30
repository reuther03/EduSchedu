using EduSchedu.Modules.Schools.Domain.Schools;

namespace EduSchedu.Modules.Schools.Application.Features.Dtos;

public class ClassDto
{
    public string Name { get; init; } = null!;
    public LanguageProficiencyDto? LanguageProficiency { get; init; }
    public List<LessonDto> Lessons { get; init; } = [];

    public static ClassDto AsDto(Class @class)
    {
        return new ClassDto
        {
            Name = @class.Name.Value,
            LanguageProficiency = LanguageProficiencyDto.AsDto(@class.LanguageProficiency!),
            Lessons = @class.Lessons.Select(LessonDto.AsDto).ToList()
        };
    }
}