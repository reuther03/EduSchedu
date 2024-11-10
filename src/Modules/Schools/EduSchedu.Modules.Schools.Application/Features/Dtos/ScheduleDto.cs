using EduSchedu.Modules.Schools.Domain.Users;

namespace EduSchedu.Modules.Schools.Application.Features.Dtos;

public class ScheduleDto
{
    public Guid Id { get; init; }
    public List<ScheduleItemDateDto> ScheduleItems { get; init; } = [];
    public Guid TeacherId { get; init; }

    public static ScheduleDto AsDto(Schedule schedule)
    {
        return new ScheduleDto
        {
            Id = schedule.Id,
            ScheduleItems = schedule.ScheduleItems.Select(ScheduleItemDateDto.AsDto).ToList(),
            TeacherId = schedule.TeacherId
        };
    }
}