using Microsoft.AspNetCore.Http;

namespace Shared.Results;

public class ModelResult<T>(Result<T> result) : IResult
{
    public Task ExecuteAsync(HttpContext httpContext)
    {
        if (result.Error != null)
        {
            return TypedResults.BadRequest(result.Error).ExecuteAsync(httpContext);
        }
        else
            return TypedResults.Ok(result.Value).ExecuteAsync(httpContext);
    }

    public static implicit operator ModelResult<T>(Result<T> wrapper) => new(wrapper);
}
