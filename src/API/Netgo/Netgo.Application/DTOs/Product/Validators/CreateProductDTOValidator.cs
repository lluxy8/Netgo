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
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId cannot be empty.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(100).WithMessage("Title must be at most 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MaximumLength(500).WithMessage("Description must be at most 500 characters.");

            RuleFor(x => x.Tradable)
                .NotNull().WithMessage("Tradable status is required.");

            RuleFor(x => x.Images)
                .Must(img => img == null || img.Count <= 8)
                .WithMessage("You can upload up to 8 images.");

            RuleFor(x => x.Price)
                .Must(p => p > 0)
                .WithMessage("Price must be grater than 0.");

            RuleForEach(x => x.Images)
                .Must(file => file.Length <= 5 * 1024 * 1024) // 5 MB max
                .WithMessage("Each image must be at most 5 MB.");
        }
    }
}
