using FluentValidation;
using Microsoft.AspNetCore.Http;
using Netgo.Application.DTOs.User;
using System.IO;

namespace Netgo.Application.DTOs.User.Validators
{
    public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserDTOValidator()
        {
            // Id: must not be empty
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User Id cannot be empty.");

            // FirstName: required, max 50 chars
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name cannot be empty.")
                .MaximumLength(50).WithMessage("First name must be at most 50 characters.");

            // LastName: required, max 50 chars
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name cannot be empty.")
                .MaximumLength(50).WithMessage("Last name must be at most 50 characters.");

            // ContactInfo: optional, if provided must be a valid phone number
            RuleFor(x => x.ContactInfo)
                .MaximumLength(100).WithMessage("Contact info must be at most 100 characters.")
                .Matches(@"^\+?[0-9\s\-\(\)]{7,20}$")
                .When(x => !string.IsNullOrWhiteSpace(x.ContactInfo))
                .WithMessage("Contact info must be a valid phone number (e.g., +905551112233).");

            // ProfilePicture: optional, max 5MB, allowed file types: jpg, jpeg, png
            RuleFor(x => x.ProfilePicture)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Profile picture must be at most 5 MB.")
                .Must(file => file == null || new[] { ".jpg", ".jpeg", ".png" }
                    .Contains(Path.GetExtension(file.FileName).ToLower()))
                .WithMessage("Profile picture must be a JPG or PNG image.");
        }
    }
}
