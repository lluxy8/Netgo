using FluentValidation;

namespace Netgo.Application.DTOs.Product.Validators
{
    public class UpdateProductDTOValidator : AbstractValidator<UpdateProductDTO>
    {
        public UpdateProductDTOValidator()
        {
            // Id: must not be empty
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Product Id cannot be empty.");

            // Title: required, max 100 chars
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(100).WithMessage("Title must be at most 100 characters.");

            // Description: required, max 500 chars
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MaximumLength(500).WithMessage("Description must be at most 500 characters.");

            // Tradable: required
            RuleFor(x => x.Tradable)
                .NotNull().WithMessage("Tradable status is required.");

            /*
            // Details: optional, if provided must contain at least one item
            RuleFor(x => x.Details)
                .Must(d => d == null || d.Count > 0)
                .WithMessage("Details must contain at least one item if provided.");
            */

            // NewImages: optional, max 5 files, each max 5MB
            RuleFor(x => x.NewImages)
                .Must(img => img == null || img.Count <= 5)
                .WithMessage("You can upload up to 5 new images.");

            RuleForEach(x => x.NewImages)
                .Must(file => file.Length <= 5 * 1024 * 1024) // 5 MB max
                .WithMessage("Each new image must be at most 5 MB.");

            // Images: optional, existing images represented by URLs or file names
            RuleFor(x => x.Images)
                .Must(list => list == null || list.All(s => !string.IsNullOrWhiteSpace(s)))
                .WithMessage("Existing images must be valid strings.");
        }
    }
}
