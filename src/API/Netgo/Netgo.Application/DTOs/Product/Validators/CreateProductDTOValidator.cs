using FluentValidation;
using Microsoft.AspNetCore.Http;
using Netgo.Application.DTOs.Product;
using System.Linq;

namespace Netgo.Application.DTOs.Product.Validators
{
    public class CreateProductDTOValidator : AbstractValidator<CreateProductDTO>
    {
        public CreateProductDTOValidator()
        {
            // UserId: must not be empty
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId cannot be empty.");

            // Title: required, max 100 chars
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(100).WithMessage("Title must be at most 100 characters.");

            // Description: required, max 500 chars
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MaximumLength(500).WithMessage("Description must be at most 500 characters.");

            // Tradable: required (bool non-nullable, but explicit rule)
            RuleFor(x => x.Tradable)
                .NotNull().WithMessage("Tradable status is required.");

            // Details: optional, if provided must have at least one item
            RuleFor(x => x.Details)
                .Must(d => d == null || d.Count > 0)
                .WithMessage("Details must contain at least one item if provided.");

            // Images: optional, if provided must not exceed 5 files and max 5MB per file
            RuleFor(x => x.Images)
                .Must(img => img == null || img.Count <= 5)
                .WithMessage("You can upload up to 5 images.");

            RuleForEach(x => x.Images)
                .Must(file => file.Length <= 5 * 1024 * 1024) // 5 MB max
                .WithMessage("Each image must be at most 5 MB.");
        }
    }
}
