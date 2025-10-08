using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Interceptors;

namespace Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services,
        IConfiguration configuration)
    {

        // Application use cases Services
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.Decorate<IBasketRepository, CachedBasketRepository>();

        // Infrastructure services
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<BasketDbContext>((sp, opts) =>
        {
            opts.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            opts.UseNpgsql(configuration.GetConnectionString("Database"));
        });

        return services;
    }

    public static async Task<IApplicationBuilder> UseBasketModule(this IApplicationBuilder app)
    {

        await app.UseMigration<BasketDbContext>();
        return app;
    }

}
