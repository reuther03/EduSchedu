using EduSchedu.Modules.Schedules.Application.Abstractions;
using EduSchedu.Shared.Infrastructure.Postgres;
using MediatR;

namespace EduSchedu.Modules.Schedules.Infrastructure.Database;

internal class SchedulesUnitOfWork : BaseUnitOfWork<SchedulesDbContext>, IUnitOfWork
{
    public SchedulesUnitOfWork(SchedulesDbContext dbContext, IPublisher publisher) : base(dbContext, publisher)
    {
    }
}