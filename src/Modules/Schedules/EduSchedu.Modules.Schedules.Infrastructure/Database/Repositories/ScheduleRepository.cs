using EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;
using EduSchedu.Modules.Schedules.Domain.Schedules;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schedules.Infrastructure.Database.Repositories;

internal class ScheduleRepository : Repository<Schedule, SchedulesDbContext>, IScheduleRepository
{
    private readonly SchedulesDbContext _dbContext;

    public ScheduleRepository(SchedulesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Schedule?> GetScheduleByUserIdAsync(UserId userId, CancellationToken cancellationToken)
        => await _dbContext.Schedules.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

    public Task<List<Schedule>> GetSchedulesByUserIdsAsync(List<UserId> userIds, CancellationToken cancellationToken)
        => _dbContext.Schedules
            .Where(x => userIds.Contains(x.UserId))
            .ToListAsync(cancellationToken);

    public async Task<bool> IsUserAvailableAsync(UserId userId, DayOfWeek day, TimeOnly start, TimeOnly end, CancellationToken cancellationToken)
        => await _dbContext.Schedules
            .Where(x => x.UserId == userId)
            .AllAsync(x => !x.ScheduleItems.Any(y => y.Day == day && y.StartTime <= start && y.EndTime >= end), cancellationToken);
            // .AllAsync(x => x.ScheduleItems.Any(y => y.Day == day && y.StartTime <= start && y.EndTime >= end), cancellationToken);
}