using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Infrastructure.Database;

internal class SchoolsDbContext : DbContext, ISchoolsDbContext
{
    public SchoolsDbContext(DbContextOptions<SchoolsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("schools");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}