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
}