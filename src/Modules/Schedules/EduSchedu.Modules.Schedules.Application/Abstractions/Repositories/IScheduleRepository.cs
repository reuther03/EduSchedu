using EduSchedu.Modules.Schedules.Domain.Schedules;
using EduSchedu.Shared.Abstractions.Kernel.Database;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;

public interface IScheduleRepository : IRepository<Schedule>
{
    Task<Schedule?> GetScheduleByUserIdAsync(UserId userId, CancellationToken cancellationToken);

    #region User

    Task<List<Schedule>> GetSchedulesByUserIdsAsync(List<UserId> userIds, CancellationToken cancellationToken);

    #endregion
}