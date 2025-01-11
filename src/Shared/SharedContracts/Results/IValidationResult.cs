namespace SharedContracts.Results;

public interface IValidationResult
{
    Error[] Errors { get; }
}
