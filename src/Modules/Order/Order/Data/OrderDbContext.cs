using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Order.Data;
public class OrderDbContext(DbContextOptions<OrderDbContext> options) 
    : DbContext(options)
{
    public DbSet<Orders.Models.Order> Orders => Set<Orders.Models.Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("order");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
