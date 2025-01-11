using Microsoft.AspNetCore.Http;

namespace SharedContracts.Results;

public class EndpointResult<T>(Result<T> result) : IResult
{
    public Task ExecuteAsync(HttpContext httpContext) =>
        result switch
        {
            { IsSuccess: true } => TypedResults.Ok(result.Value).ExecuteAsync(httpContext),
            IValidationResult validationResult => TypedResults.BadRequest(validationResult.Errors).ExecuteAsync(httpContext),
            _ => TypedResults.BadRequest(result.Error).ExecuteAsync(httpContext)
        };

    public static implicit operator EndpointResult<T>(Result<T> wrapper) => new(wrapper);
}
