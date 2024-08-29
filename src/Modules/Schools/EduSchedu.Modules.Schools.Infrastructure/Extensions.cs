using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Infrastructure.Database;
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
            .AddUnitOfWork<IUnitOfWork, UserUnitOfWork>();

        return services;
    }
}