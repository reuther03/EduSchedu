using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Domain.Users;

namespace EduSchedu.Modules.Schools.Application.Features.Dtos;

public class ScheduleItemDateDto
{
    public Guid Id { get; init; }
    public DayOfWeek Day { get; init; }
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }

    public static ScheduleItemDateDto AsDto(ScheduleItem scheduleItem)
    {
        return new ScheduleItemDateDto
        {
            Id = scheduleItem.Id,
            Day = scheduleItem.Day,
            Start = scheduleItem.Start,
            End = scheduleItem.End
        };
    }
}