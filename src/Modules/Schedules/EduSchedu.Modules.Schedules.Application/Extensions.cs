﻿using Microsoft.Extensions.DependencyInjection;

namespace EduSchedu.Modules.Schedules.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}