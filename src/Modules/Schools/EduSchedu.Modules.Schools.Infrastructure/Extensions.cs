using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Infrastructure.Database;
using EduSchedu.Modules.Schools.Infrastructure.Database.Repositories;
using EduSchedu.Shared.Abstractions.Services;
using EduSchedu.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace EduSchedu.Modules.Schools.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddPostgres<SchoolsDbContext>()
            .AddScoped<ISchoolsDbContext, SchoolsDbContext>()
            .AddScoped<ISchoolUserRepository, SchoolUserRepository>()
            .AddScoped<ISchoolRepository, SchoolRepository>()
            .AddScoped<ILanguageProficiencyRepository, LanguageProficiencyRepository>()
            .AddUnitOfWork<ISchoolUnitOfWork, SchoolUnitOfWork>()
            .AddTransient<IModuleSeeder, SchoolModuleSeeder>();

        // services.AddHostedService<ScheduleItemJob>();

        return services;
    }
}