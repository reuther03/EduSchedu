using EduSchedu.Modules.Schedules.Application.Abstractions;
using EduSchedu.Modules.Schedules.Application.Abstractions.Database;
using EduSchedu.Modules.Schedules.Infrastructure.Database;
using EduSchedu.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace EduSchedu.Modules.Schedules.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddPostgres<SchedulesDbContext>()
            .AddScoped<ISchedulesDbContext, SchedulesDbContext>()
            .AddUnitOfWork<IUnitOfWork, SchedulesUnitOfWork>();

        return services;
    }
}