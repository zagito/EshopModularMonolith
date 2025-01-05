namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand( 
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    decimal Price,
    string ImageFile)
    : ICommand;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public class UpdateProductHandler(CatalogDbContext dbContext) : ICommandHandler<UpdateProductCommand> 
{
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken) 
    {
        Product? product = await dbContext.Products.FindAsync([request.Id], cancellationToken: cancellationToken);

        if (product is null)
            return Error.ProductNotFound;

        product.Update(
            request.Name,
            request.Category,
            request.Description,
            request.ImageFile,
            request.Price);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
