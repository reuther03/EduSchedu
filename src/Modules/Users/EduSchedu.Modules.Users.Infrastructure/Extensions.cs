using EduSchedu.Modules.Users.Application.Abstractions;
using EduSchedu.Modules.Users.Infrastructure.Database;
using EduSchedu.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace EduSchedu.Modules.Users.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddPostgres<UsersDbContext>()
            .AddUnitOfWork<IUnitOfWork, UserUnitOfWork>();

        return services;
    }
}