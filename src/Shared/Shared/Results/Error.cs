namespace Shared.Results;

public record Error(string Code, string Message) 
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error ProductNotFound = new("404", "Product Not Found");
    public static readonly Error ValidationError = new("400", "Validation Error");
}
