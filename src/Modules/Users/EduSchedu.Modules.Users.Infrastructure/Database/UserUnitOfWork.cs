using EduSchedu.Modules.Users.Application.Abstractions;
using EduSchedu.Shared.Infrastructure.Postgres;
using MediatR;

namespace EduSchedu.Modules.Users.Infrastructure.Database;

internal class UserUnitOfWork : BaseUnitOfWork<UsersDbContext>, IUserUnitOfWork
{
    public UserUnitOfWork(UsersDbContext dbContext, IPublisher publisher) : base(dbContext, publisher)
    {
    }
}