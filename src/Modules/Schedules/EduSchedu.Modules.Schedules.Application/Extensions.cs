using EduSchedu.Modules.Schedules.Application.Abstractions.Services;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EduSchedu.Modules.Schedules.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}