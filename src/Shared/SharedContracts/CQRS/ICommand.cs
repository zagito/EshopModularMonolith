using MediatR;
using SharedContracts.Results;

namespace SharedContracts.CQRS;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
