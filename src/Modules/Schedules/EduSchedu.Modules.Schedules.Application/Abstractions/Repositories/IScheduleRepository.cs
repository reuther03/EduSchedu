using EduSchedu.Modules.Schedules.Domain.Schedules;
using EduSchedu.Shared.Abstractions.Integration.Events.EventPayloads;
using EduSchedu.Shared.Abstractions.Kernel.Database;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;

public interface IScheduleRepository : IRepository<Schedule>
{
    Task<Schedule?> GetScheduleByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<List<Schedule>> GetSchedulesByUserIdsAsync(List<UserId> userIds, CancellationToken cancellationToken);
    Task<bool> IsUserAvailableAsync(UserId userId, DayOfWeek day, TimeOnly start, TimeOnly end, CancellationToken cancellationToken);

    Task<List<UserId>> GetAvailableTeachersByScheduleItemsAsync(List<ScheduleItemPayload> scheduleItems, List<UserId> teachersIds,
        CancellationToken cancellationToken);
}