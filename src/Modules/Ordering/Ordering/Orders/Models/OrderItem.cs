﻿namespace Ordering.Orders.Models;

public class OrderItem : Entity<Guid>
{
    internal OrderItem(Guid orderId, Guid productId, int quantity, decimal price)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; internal set; }
    public decimal Price { get; private set; }
}
