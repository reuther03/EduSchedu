using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Shared.Infrastructure.Postgres;
using MediatR;

namespace EduSchedu.Modules.Schools.Infrastructure.Database;

internal class UnitOfWork : BaseUnitOfWork<SchoolsDbContext>, IUnitOfWork
{
    public UnitOfWork(SchoolsDbContext dbContext, IPublisher publisher) : base(dbContext, publisher)
    {
    }
}