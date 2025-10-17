using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extensions;
public static class MediatorExtensions
{
    public static IServiceCollection AddMediatorAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        services.AddMediatR(config =>
        {
            var requiredAssemblies = assemblies
                .Where(assembly => !assembly.IsDynamic && !string.IsNullOrWhiteSpace(assembly.Location))
                .ToArray();

            config.RegisterServicesFromAssemblies(requiredAssemblies);
            config.AddOpenBehavior(typeof(Behaviors.ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(Behaviors.LoggingBahavior<,>));
        });

        services.AddValidatorsFromAssemblies(assemblies);
        
        return services;
    }
}
