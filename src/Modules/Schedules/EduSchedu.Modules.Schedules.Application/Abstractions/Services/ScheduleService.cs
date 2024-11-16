using EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schedules.Application.Abstractions.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository;

    public ScheduleService(IScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }

    public async Task<bool> IsUserAvailableAsync(UserId userId, DayOfWeek day, TimeOnly start, TimeOnly end, CancellationToken cancellationToken)
        => await _scheduleRepository.IsUserAvailableAsync(userId, day, start, end, cancellationToken);
}