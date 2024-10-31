using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Domain.Schools;

namespace EduSchedu.Modules.Schools.Application.Features.Dtos;

public class LessonDto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DayOfWeek Day { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public Guid? AssignedTeacher { get; init; }

    public static LessonDto AsDto(Lesson lesson)
    {
        return new LessonDto
        {
            Day = lesson.Day,
            StartTime = lesson.StartTime,
            EndTime = lesson.EndTime,
            AssignedTeacher = lesson.AssignedTeacher?.Value
        };
    }
}