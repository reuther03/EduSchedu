using EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;
using EduSchedu.Modules.Schedules.Domain.Schedules;
using EduSchedu.Shared.Infrastructure.Postgres;

namespace EduSchedu.Modules.Schedules.Infrastructure.Database.Repositories;

internal class ScheduleRepository : Repository<Schedule, SchedulesDbContext>, IScheduleRepository
{
    public ScheduleRepository(SchedulesDbContext dbContext) : base(dbContext)
    {
    }
}