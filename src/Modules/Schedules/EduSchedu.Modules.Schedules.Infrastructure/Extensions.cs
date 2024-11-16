using EduSchedu.Modules.Schedules.Application.Abstractions;
using EduSchedu.Modules.Schedules.Application.Abstractions.Database;
using EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;
using EduSchedu.Modules.Schedules.Application.Abstractions.Services;
using EduSchedu.Modules.Schedules.Infrastructure.Database;
using EduSchedu.Modules.Schedules.Infrastructure.Database.Repositories;
using EduSchedu.Shared.Abstractions.Services;
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
            .AddScoped<IScheduleService, ScheduleService>()
            .AddScoped<IScheduleRepository, ScheduleRepository>()
            .AddUnitOfWork<IUnitOfWork, SchedulesUnitOfWork>();

        return services;
    }
}