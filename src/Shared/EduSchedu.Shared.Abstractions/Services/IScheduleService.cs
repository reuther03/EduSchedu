using EduSchedu.Shared.Abstractions.Integration.Events.EventPayloads;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Shared.Abstractions.Services;

public interface IScheduleService
{
    Task<bool> IsUserAvailableAsync(UserId userId, DayOfWeek day, TimeOnly start, TimeOnly end, CancellationToken cancellationToken);

    Task<List<UserId>> GetAvailableTeachersByScheduleItemsAsync(List<ScheduleItemPayload> scheduleItems, List<UserId> teachersIds,
        CancellationToken cancellationToken);
}