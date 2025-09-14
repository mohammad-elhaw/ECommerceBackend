using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.seed;

namespace Shared.Data;
public static class Extensions
{
    public static async Task<IApplicationBuilder> UseMigration<TContext>
        (this IApplicationBuilder app) where TContext : DbContext
    {
        
        await MigrateDatabase<TContext>(app.ApplicationServices);
        await SeedData(app.ApplicationServices);

        return app;
    }

    private static async Task MigrateDatabase<TContext>(IServiceProvider sp)
        where TContext : DbContext
    {
        using var scope = sp.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.MigrateAsync();
    }

    private static async Task SeedData(IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var seeder in seeders)
        {
            await seeder.SeedAll();
        }
    }
}
