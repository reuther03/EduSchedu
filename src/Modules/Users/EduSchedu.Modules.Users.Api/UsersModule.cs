using EduSchedu.Modules.Users.Application;
using EduSchedu.Modules.Users.Domain;
using EduSchedu.Modules.Users.Infrastructure;
using EduSchedu.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EduSchedu.Modules.Users.Api;

public class UsersModule : IModule
{
    public const string BasePath = "users-module";

    public string Name { get; } = "Users";
    public string Path => BasePath;

    public void Register(IServiceCollection services)
    {
        services
            .AddDomain()
            .AddApplication()
            .AddInfrastructure();
    }

    public void Use(IApplicationBuilder app)
    {
    }
}