using MediatR;
using SharedContracts.Results;

namespace SharedContracts.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
