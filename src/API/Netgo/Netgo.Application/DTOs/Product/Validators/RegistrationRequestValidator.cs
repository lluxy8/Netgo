using FluentValidation;
using Netgo.Application.Models.Identity;

namespace Netgo.Application.DTOs.Product.Validators
{
    public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
    {
        public RegistrationRequestValidator()
        {
            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("First name cannot be empty.")
                .MaximumLength(50).WithMessage("First name must be at most 50 characters.");

            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("Last name cannot be empty.")
                .MaximumLength(50).WithMessage("Last name must be at most 50 characters.");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Please enter a valid email address.")
                .MaximumLength(256).WithMessage("Email must be at most 256 characters.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .MaximumLength(100).WithMessage("Password must be at most 100 characters.")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least 1 uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least 1 lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain at least 1 number.")
                .Matches(@"[\!\?\*\.@#$%^&+=]+").WithMessage("Password must contain at least 1 special character (e.g., !?*.@#$%^&+=).");
        }
    }
}
