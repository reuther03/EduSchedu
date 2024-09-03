using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Shared.Infrastructure.Postgres;
using MediatR;

namespace EduSchedu.Modules.Schools.Infrastructure.Database;

internal class SchoolUnitOfWork : BaseUnitOfWork<SchoolsDbContext>, ISchoolUnitOfWork
{
    public SchoolUnitOfWork(SchoolsDbContext dbContext, IPublisher publisher) : base(dbContext, publisher)
    {
    }
}