using Microsoft.AspNetCore.Http;

namespace Shared.Results;

public class EndpointResult(Result result) : IResult
{
    public Task ExecuteAsync(HttpContext httpContext) =>
        result switch
        {
            { IsSuccess: true } => TypedResults.Ok().ExecuteAsync(httpContext),
            IValidationResult validationResult => TypedResults.BadRequest(validationResult.Errors).ExecuteAsync(httpContext),
            _ => TypedResults.BadRequest(result.Error).ExecuteAsync(httpContext)
        };

    public static implicit operator EndpointResult(Result result) => new(result);
}
