using EduSchedu.Modules.Schools.Domain.Users;

namespace EduSchedu.Modules.Schools.Application.Features.Dtos;

public class ScheduleItemDto
{
    public Guid Id { get; init; }
    public ScheduleItemType Type { get; init; }
    public DayOfWeek Day { get; init; }
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }
    public string Description { get; init; } = string.Empty;

    public static ScheduleItemDto AsDto(ScheduleItem scheduleItem)
    {
        return new ScheduleItemDto
        {
            Id = scheduleItem.Id,
            Type = scheduleItem.Type,
            Day = scheduleItem.Day,
            Start = scheduleItem.Start,
            End = scheduleItem.End,
            Description = scheduleItem.Description
        };
    }
}