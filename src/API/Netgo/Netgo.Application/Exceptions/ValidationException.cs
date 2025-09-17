using FluentValidation.Results;

namespace Netgo.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public List<string> Messages { get; set; } = [];
        public ValidationException(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Messages.Add(error.ErrorMessage);
            }
        }
    }
}
