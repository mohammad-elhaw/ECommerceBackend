using Microsoft.EntityFrameworkCore;
using Shared.Data.seed;

namespace Catalog.Data.Seed;
public class CatalogDataSeeder(CatalogDbContext context) : IDataSeeder
{
    public async Task SeedAll()
    {
        if(!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(InitialData.Products);
            await context.SaveChangesAsync();
        }
    }
}
