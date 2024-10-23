﻿using System.Reflection;
using System.Runtime.CompilerServices;
using EduSchedu.Shared.Abstractions.Email;
using EduSchedu.Shared.Abstractions.Modules;
using EduSchedu.Shared.Abstractions.Services;
using EduSchedu.Shared.Infrastructure.Api;
using EduSchedu.Shared.Infrastructure.Auth;
using EduSchedu.Shared.Infrastructure.Postgres;
using EduSchedu.Shared.Infrastructure.Services;
using EduSchedu.Shared.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("EduSchedu.Bootstrapper")]

namespace EduSchedu.Shared.Infrastructure;

internal static class Extensions
{
    private const string CorsPolicy = "cors";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IList<Assembly> assemblies, IList<IModule> modules,
        IConfiguration configuration)
    {
        var disabledModules = new List<string>();
        // using (var serviceProvider = services.BuildServiceProvider())
        // var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        foreach (var (key, value) in configuration.AsEnumerable())
        {
            if (!key.Contains(":module:enabled"))
            {
                continue;
            }

            if (value != null && !bool.Parse(value))
            {
                disabledModules.Add(key.Split(":")[0]);
            }
        }

        services.AddCors(cors =>
        {
            cors.AddPolicy(CorsPolicy, x =>
            {
                x.WithOrigins("http://localhost:5000", "https://localhost:5000")
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        services.AddSwagger();
        services.AddAuth(configuration);
        services.AddDecorators();
        services.AddHostedService<AppInitializer>();
        services.Configure<EmailSettings>(configuration.GetRequiredSection(EmailSettings.SectionName));
        services.AddServices();
        services.AddPostgres();
        services.AddMediatrWithFilters(assemblies);

        services.AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                var removedParts = new List<ApplicationPart>();
                foreach (var disabledModule in disabledModules)
                {
                    var parts = manager.ApplicationParts.Where(x => x.Name.Contains(disabledModule, StringComparison.InvariantCultureIgnoreCase));
                    removedParts.AddRange(parts);
                }

                foreach (var part in removedParts)
                {
                    manager.ApplicationParts.Remove(part);
                }

                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseCors(CorsPolicy);
        app.UseAuthentication();
        app.UseAuthorization();
        // app.UseEndpoints(endpoints =>
        // {
        //     endpoints.MapControllers();
        //     endpoints.MapHub<ClassChatHub>("/chat");
        // });
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EduSchedu API"); });
        return app;
    }

    public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        return configuration.GetOptions<T>(sectionName);
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }

    public static string GetModuleName(this object value)
        => value?.GetType().GetModuleName() ?? string.Empty;

    public static string GetModuleName(this Type type)
    {
        if (type?.Namespace is null)
        {
            return string.Empty;
        }

        return type.Namespace.StartsWith("TaskManager.Modules.")
            ? type.Namespace.Split('.')[2].ToLowerInvariant()
            : string.Empty;
    }
}