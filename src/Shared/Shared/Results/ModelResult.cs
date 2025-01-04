using Microsoft.AspNetCore.Http;

namespace Shared.Results;

public class ModelResult(Result result) : IResult
{
    public Task ExecuteAsync(HttpContext httpContext)
    {
        if (result.Error != null)
        {
            return TypedResults.BadRequest(result.Error).ExecuteAsync(httpContext);
        }
        else
            return TypedResults.Ok().ExecuteAsync(httpContext);
    }

    public static implicit operator ModelResult(Result result) => new(result);
}
