namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    decimal Price,
    string ImageFile)
    : ICommand<Guid>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public class CreateProductHandler(
    CatalogDbContext dbContext,
    ILogger<CreateProductHandler> logger)
    : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        //var result = await validator.ValidateAsync(command, cancellationToken);
        //var errors = result.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)).ToArray();
        //if (errors.Any())
        //{
        //    return errors;
        //}

        logger.LogInformation("CreateProductHandler.Handle called with {@Command}", command);

        Product product = Product.Create(
            command.Name,
            command.Category,
            command.Description,
            command.ImageFile,
            command.Price);

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
