using EduSchedu.Modules.Users.Application.Abstractions;
using EduSchedu.Shared.Infrastructure.Postgres;
using MediatR;

namespace EduSchedu.Modules.Users.Infrastructure.Database;

internal class UnitOfWork : BaseUnitOfWork<UsersDbContext>, IUnitOfWork
{
    public UnitOfWork(UsersDbContext dbContext, IPublisher publisher) : base(dbContext, publisher)
    {
    }
}