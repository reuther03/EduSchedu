using EduSchedu.Modules.Schools.Application;
using EduSchedu.Modules.Schools.Domain;
using EduSchedu.Modules.Schools.Infrastructure;
using EduSchedu.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EduSchedu.Modules.Schools.Api;

public class SchoolsModule : IModule
{
    public const string BasePath = "schools-module";

    public string Name { get; } = "School";
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