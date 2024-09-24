using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Modules.Schools.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EduSchedu.Modules.Schools.Infrastructure.Jobs;

public class ScheduleItemJob : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ScheduleItemJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SchoolsDbContext>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var schedules = await context.SchoolUsers
                .OfType<Teacher>()
                .Include(x => x.Schedule)
                .ThenInclude(x => x.ScheduleItems)
                .Select(x => x.Schedule)
                .ToListAsync(stoppingToken);

            foreach (var schedule in schedules)
            {
                var test = schedule.ScheduleItems.Where(x => x.Day == DateTime.Now.DayOfWeek &&
                        x.End.ToTimeSpan() <= DateTime.Now.TimeOfDay &&
                        x.Type != ScheduleItemType.Lesson)
                    .ToList();

                test.ForEach(x => schedule.RemoveScheduleItem(x));
            }

            await context.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}