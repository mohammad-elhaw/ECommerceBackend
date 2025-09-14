using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations;
public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Categories).IsRequired();
        builder.Property(p => p.Description).IsRequired().HasMaxLength(200);
        builder.Property(p => p.ImageFile).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
    }
}
