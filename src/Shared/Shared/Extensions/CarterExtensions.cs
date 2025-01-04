﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Carter;

namespace Shared.Extensions;

public static class CarterExtensions
{
    public static IServiceCollection AddCarterWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddCarter(configurator: config  =>
        {
            foreach (Assembly assembly in assemblies)
            {
                var modules = assembly.GetTypes()
                    .Where(x => x.IsAssignableTo(typeof(ICarterModule)))
                    .ToArray();

                config.WithModules(modules);
            }
        });

        return services;
    }
}
