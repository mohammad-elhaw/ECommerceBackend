using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Data.Configurations;
internal class OrderConfig : IEntityTypeConfiguration<Orders.Models.Order>
{
    public void Configure(EntityTypeBuilder<Orders.Models.Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.OrderName)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(o => o.OrderName)
            .IsUnique();

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(o => o.ShippingAddress, sa =>
        {
            sa.Property(a => a.EmailAddress).IsRequired().HasMaxLength(100);
            sa.Property(a => a.AddressLine).IsRequired().HasMaxLength(200);
            sa.Property(a => a.Country).IsRequired().HasMaxLength(50);
            sa.Property(a => a.State).IsRequired().HasMaxLength(50);
            sa.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
        });

        builder.OwnsOne(o => o.BillingAddress, ba =>
        {
            ba.Property(a => a.EmailAddress).IsRequired().HasMaxLength(100);
            ba.Property(a => a.AddressLine).IsRequired().HasMaxLength(200);
            ba.Property(a => a.Country).IsRequired().HasMaxLength(50);
            ba.Property(a => a.State).IsRequired().HasMaxLength(50);
            ba.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
        });

        builder.OwnsOne(o => o.Payment, p =>
        {
            p.Property(py => py.CardName).IsRequired().HasMaxLength(100);
            p.Property(py => py.CardNumber).IsRequired().HasMaxLength(30);
            p.Property(py => py.Expiration).IsRequired().HasMaxLength(10);
            p.Property(py => py.CVV).IsRequired().HasMaxLength(3);
        });
    }
}
