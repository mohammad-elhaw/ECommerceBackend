using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Orders.Models;

namespace Order.Data.Configurations;
internal class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);
        builder.Property(oi => oi.Quantity)
               .IsRequired();

        builder.Property(oi => oi.Price)
            .IsRequired();

        builder.Property(oi => oi.OrderId)
            .IsRequired();
    }
}
