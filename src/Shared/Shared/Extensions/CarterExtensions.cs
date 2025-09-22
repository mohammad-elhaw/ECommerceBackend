using Carter;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions;
public static class CarterExtensions
{
    public static IServiceCollection AddCarterWithAssemblies(this IServiceCollection services)
    {
        services.AddCarter(configurator: config =>
        {
            var carterModules = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ICarterModule).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                .ToArray();
            config.WithModules(carterModules);
        });
        return services;
    }
}
