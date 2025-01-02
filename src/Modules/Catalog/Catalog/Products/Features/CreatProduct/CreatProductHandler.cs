using MediatR;

namespace Catalog.Products.Features.CreatProduct;

public record CreatProductComand(
    string Name,
    List<string> Category,
    string Description,
    decimal Price,
    string ImageFile)
    : IRequest<CreatProductResult>;

public record CreatProductResult(Guid Id);

public class CreatProductComandHandler : IRequestHandler<CreatProductComand, CreatProductResult>
{
    public Task<CreatProductResult> Handle(CreatProductComand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
