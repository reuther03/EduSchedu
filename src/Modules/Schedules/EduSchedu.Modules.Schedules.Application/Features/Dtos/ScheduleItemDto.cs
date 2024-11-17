using EduSchedu.Modules.Schedules.Domain.Schedules;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schedules.Application.Features.Dtos;

public class ScheduleItemDto
{
    public ScheduleItemType Type { get; init; }
    public DayOfWeek Day { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }

    public static ScheduleItemDto AsDto(ScheduleItem scheduleItem)
    {
        return new ScheduleItemDto
        {
            Type = scheduleItem.Type,
            Day = scheduleItem.Day,
            StartTime = scheduleItem.StartTime,
            EndTime = scheduleItem.EndTime
        };
    }
}