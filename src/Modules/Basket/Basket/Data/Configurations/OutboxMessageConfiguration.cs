using Basket.ShoppingCarts.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.Data.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(om => om.Id);

        builder.Property(om => om.Type).IsRequired()
            .HasMaxLength(100);

        builder.Property(om => om.Content).IsRequired()
            .HasMaxLength(200);

        builder.Property(om => om.OccurredOn).IsRequired();
    }
}
