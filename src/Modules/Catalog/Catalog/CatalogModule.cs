using Catalog.Data.Seed;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Interceptors;
using Shared.Data.seed;
using System.Reflection;

namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services,
        IConfiguration configuration)
    {

        // Api Endpoint service

        // Application use case services
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Infrastructure services

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

        services.AddDbContext<CatalogDbContext>((sp, opts) =>
        {
            opts.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            opts.UseNpgsql(configuration.GetConnectionString("Database"));
        });

        services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        return services;
    }

    public static async Task<IApplicationBuilder> UseCatalogModule(this IApplicationBuilder app)
    {

        // use Api Endpoints
        
        // use Application use case services

        // use Infrastructure services

        await app.UseMigration<CatalogDbContext>();

        return app;
    }
}
