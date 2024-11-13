using EduSchedu.Modules.Schedules.Application.Abstractions.Database;
using EduSchedu.Modules.Schedules.Domain.Schedules;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schedules.Infrastructure.Database;

internal class SchedulesDbContext : DbContext, ISchedulesDbContext
{
    public DbSet<Schedule> Schedules => Set<Schedule>();

    public SchedulesDbContext(DbContextOptions<SchedulesDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("schedules");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}