using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions;
public static class MediatorExtensions
{
    public static IServiceCollection AddMediatorAssemblies(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic && !string.IsNullOrWhiteSpace(assembly.Location))
                .ToArray();

            config.RegisterServicesFromAssemblies(assemblies);
            config.AddOpenBehavior(typeof(Behaviors.ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(Behaviors.LoggingBahavior<,>));
        });

        return services;
    }
}
