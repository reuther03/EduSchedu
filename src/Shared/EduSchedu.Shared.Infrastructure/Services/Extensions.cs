using EduSchedu.Shared.Abstractions.Services;
using EduSchedu.Shared.Infrastructure.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using IEmailSender = EduSchedu.Shared.Abstractions.Email.IEmailSender;

namespace EduSchedu.Shared.Infrastructure.Services;

internal static class Extensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IEmailSender, EmailSender>();
        services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        return services;
    }
}