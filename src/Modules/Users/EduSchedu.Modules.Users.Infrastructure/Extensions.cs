using EduSchedu.Modules.Users.Application.Abstractions;
using EduSchedu.Modules.Users.Application.Abstractions.Database;
using EduSchedu.Modules.Users.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Users.Infrastructure.Database;
using EduSchedu.Modules.Users.Infrastructure.Database.Repositories;
using EduSchedu.Shared.Abstractions.Services;
using EduSchedu.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace EduSchedu.Modules.Users.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddPostgres<UsersDbContext>()
            .AddScoped<IUserDbContext, UsersDbContext>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddUnitOfWork<IUserUnitOfWork, UserUnitOfWork>()
            .AddTransient<IModuleSeeder, UsersModuleSeeder>();

        return services;
    }
}