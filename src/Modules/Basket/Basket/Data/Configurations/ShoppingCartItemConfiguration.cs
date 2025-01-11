using Basket.ShoppingCarts.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.Data.Configurations;

public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder.HasKey(si => si.Id);

        builder.Property(si => si.ProductId).IsRequired();

        builder.Property(si => si.Quantity).IsRequired();

        builder.Property(si => si.Color).IsRequired()
            .HasMaxLength(50);

        builder.Property(si => si.Price).IsRequired();

        builder.Property(si => si.ProductName).IsRequired()
            .HasMaxLength(200);
    }
}
