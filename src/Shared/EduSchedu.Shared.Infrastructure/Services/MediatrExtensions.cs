using System.Reflection;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Infrastructure.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace EduSchedu.Shared.Infrastructure.Services;

public static class MediatrExtensions
{
    public static IServiceCollection AddMediatrWithFilters(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        // Add MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies.ToArray());
            cfg.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
        });

        // Scan assemblies for handlers
        var handlerTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                        i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))) &&
                !t.GetCustomAttributes<DecoratorAttribute>().Any());

        foreach (var handlerType in handlerTypes)
        {
            var interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                        i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)));

            foreach (var handlerInterface in interfaces)
            {
                services.AddScoped(handlerInterface, handlerType);
            }
        }

        return services;
    }
}