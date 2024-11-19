namespace EduSchedu.Shared.Abstractions.Integration.Events.EventPayloads;

public record ScheduleItemPayload
{
    public DayOfWeek Day { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
}