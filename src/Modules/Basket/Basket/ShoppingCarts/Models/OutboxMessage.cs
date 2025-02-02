namespace Basket.ShoppingCarts.Models;

public class OutboxMessage : Entity<Guid>
{
    public string Type { get; private set; } = default!;
    public string Content { get; private set; } = default!;
    public DateTime OccurredOn { get; private set; } = default!;    
    public DateTime? ProcessedOn { get; private set; } = default!;

    private OutboxMessage()
    {
    }

    public static OutboxMessage Create(string type, string content)
    {
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = type,
            Content = content,
            OccurredOn = DateTime.UtcNow,
        };
    }

    public void MarkAsProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
    }
}
