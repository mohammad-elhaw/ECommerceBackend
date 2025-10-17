using Carter;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extensions;
public static class CarterExtensions
{
    public static IServiceCollection AddCarterWithAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        services.AddCarter(configurator: config =>
        {
            var carterModules = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ICarterModule).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                .ToArray();
            config.WithModules(carterModules);
        });
        return services;
    }
}
