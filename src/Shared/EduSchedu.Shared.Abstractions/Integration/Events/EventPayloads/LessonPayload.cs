namespace EduSchedu.Shared.Abstractions.Integration.Events.EventPayloads;

public record LessonPayload
{
    public DayOfWeek Day { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
}