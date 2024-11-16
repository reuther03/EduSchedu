using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Shared.Abstractions.Services;

public interface IScheduleService
{
    Task<bool> IsUserAvailableAsync(UserId userId, DayOfWeek day, TimeOnly start, TimeOnly end, CancellationToken cancellationToken);
}