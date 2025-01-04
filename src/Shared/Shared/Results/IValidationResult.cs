namespace Shared.Results;

public interface IValidationResult
{
    Error[] Errors { get; }
}
