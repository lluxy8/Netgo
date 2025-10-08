using FluentValidation.Results;

public class ValidationException : ApplicationException
{
    public List<string> Messages { get; } = new();

    public ValidationException(ValidationResult validationResult, string? message = null)
        : base(BuildMessage(validationResult, message))
    {
        foreach (var error in validationResult.Errors)
        {
            Messages.Add(error.ErrorMessage);
        }
    }

    private static string BuildMessage(ValidationResult validationResult, string? baseMessage)
    {
        var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);
        var combinedErrors = string.Join(" <br/>", errorMessages);

        if (!string.IsNullOrWhiteSpace(baseMessage))
            return $"{baseMessage}: {combinedErrors}";

        return combinedErrors;
    }
}
