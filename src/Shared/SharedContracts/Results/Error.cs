namespace SharedContracts.Results;

public record Error(string Code, string Message) 
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error ProductNotFound = new("404", "Product Not Found");
    public static readonly Error ValidationError = new("400", "Validation Error");
    public static readonly Error ShoppingCartNotFound = new("404", "Shopping Cart Not Found");
    public static readonly Error OrderNotFound = new("404", "Order Not Found");
    public static readonly Error ShoppingCartCheckoutFailed = new("400", "Check out failed");
}
