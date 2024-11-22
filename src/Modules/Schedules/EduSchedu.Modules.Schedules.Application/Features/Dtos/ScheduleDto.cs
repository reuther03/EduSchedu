using EduSchedu.Modules.Schedules.Domain.Schedules;

namespace EduSchedu.Modules.Schedules.Application.Features.Dtos;

public class ScheduleDto
{
    public Guid UserId { get; init; }
    public List<ScheduleItemDto> Items { get; init; } = [];

    public static ScheduleDto AsDto(Schedule schedule)
    {
        return new ScheduleDto
        {
            UserId = schedule.UserId,
            Items = schedule.ScheduleItems.Select(ScheduleItemDto.AsDto).ToList()
        };
    }
}