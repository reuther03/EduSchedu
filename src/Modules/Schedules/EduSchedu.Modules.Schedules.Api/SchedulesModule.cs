using EduSchedu.Modules.Schedules.Application;
using EduSchedu.Modules.Schedules.Domain;
using EduSchedu.Modules.Schedules.Infrastructure;
using EduSchedu.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once ClassNeverInstantiated.Global

namespace EduSchedu.Modules.Schedules.Api;

public class SchedulesModule : IModule
{
    public const string BasePath = "schedules-module";

    public string Name => "Schedules";
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