using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Infrastructure.Database;

internal class SchoolsDbContext : DbContext, ISchoolsDbContext
{
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<School> Schools => Set<School>();
    public DbSet<Teacher> Teachers => Set<Teacher>();

    public SchoolsDbContext(DbContextOptions<SchoolsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("schools");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}