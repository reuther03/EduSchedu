using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Shared.Infrastructure.Postgres;
using MediatR;

namespace EduSchedu.Modules.Schools.Infrastructure.Database;

internal class UserUnitOfWork : BaseUnitOfWork<SchoolsDbContext>, IUnitOfWork
{
    public UserUnitOfWork(SchoolsDbContext dbContext, IPublisher publisher) : base(dbContext, publisher)
    {
    }
}