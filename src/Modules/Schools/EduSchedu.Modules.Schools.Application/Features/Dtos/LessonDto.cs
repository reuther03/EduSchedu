using EduSchedu.Modules.Schools.Domain.Schools;

namespace EduSchedu.Modules.Schools.Application.Features.Dtos;

public class LessonDto
{
    public DayOfWeek Day { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public Guid? AssignedTeacher { get; set; }
    public Guid? ScheduleId { get; set; }

    public static LessonDto AsDto(Lesson lesson)
    {
        return new LessonDto
        {
            Day = lesson.Day,
            StartTime = lesson.StartTime,
            EndTime = lesson.EndTime,
            AssignedTeacher = lesson.AssignedTeacher?.Value,
            ScheduleId = lesson.ScheduleId?.Value
        };
    }
}