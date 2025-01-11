using MediatR;
using SharedContracts.Results;

namespace SharedContracts.CQRS;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
