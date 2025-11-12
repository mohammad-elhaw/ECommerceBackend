using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Data;
using Shared.Data;
using Shared.Data.Interceptors;

namespace Order;

public static class OrderModule
{
    public static IServiceCollection AddOrderModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Api Endpoint service

        // Application use case services

        // Infrastructure services

        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddDbContext<OrderDbContext>((sp, opts) =>
        {
            opts.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            opts.UseNpgsql(configuration.GetConnectionString("Database"));
        });

        return services;
    }

    public static async Task<IApplicationBuilder> UseOrderModule(this IApplicationBuilder app)
    {
        await app.UseMigration<OrderDbContext>();

        return app;
    }
}
