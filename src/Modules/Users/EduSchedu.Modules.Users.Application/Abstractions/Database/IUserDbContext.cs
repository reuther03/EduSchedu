using EduSchedu.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Users.Application.Abstractions.Database;

public interface IUserDbContext
{
    DbSet<User> Users { get; }
}