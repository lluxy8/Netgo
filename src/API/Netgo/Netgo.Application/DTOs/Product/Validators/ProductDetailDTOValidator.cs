using FluentValidation;

namespace Netgo.Application.DTOs.Product.Validators
{
    public class ProductDetailDtoValidator : AbstractValidator<ProductDetailDto>
    {
        public ProductDetailDtoValidator()
        {
            // Title: required, max 100 chars
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Detail title cannot be empty.")
                .MaximumLength(100).WithMessage("Detail title must be at most 100 characters.");

            // Value: required, max 200 chars
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Detail value cannot be empty.")
                .MaximumLength(200).WithMessage("Detail value must be at most 200 characters.");
        }
    }
}
