using EduSchedu.Shared.Abstractions.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EduSchedu.Shared.Infrastructure.Services;

internal static class Extensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IUserService, UserService>();
        services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        return services;
    }
}