using Microsoft.AspNetCore.Http;

namespace Shared.Results;

public class ModelResult<T>(Result<T> result) : IResult
{
    public Task ExecuteAsync(HttpContext httpContext) =>
        result switch
        {
            { IsSuccess: true } => TypedResults.Ok(result.Value).ExecuteAsync(httpContext),
            IValidationResult validationResult => TypedResults.BadRequest(validationResult.Errors).ExecuteAsync(httpContext),
            _ => TypedResults.BadRequest(result.Error).ExecuteAsync(httpContext)
        };

    public static implicit operator ModelResult<T>(Result<T> wrapper) => new(wrapper);
}
