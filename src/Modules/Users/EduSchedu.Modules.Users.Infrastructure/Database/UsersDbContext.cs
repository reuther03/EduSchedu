using EduSchedu.Modules.Users.Application.Abstractions.Database;
using EduSchedu.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Users.Infrastructure.Database;

internal class UsersDbContext : DbContext, IUserDbContext
{
    public DbSet<User> Users => Set<User>();

    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("users");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}