using FluentValidation;

namespace Netgo.Application.DTOs.Category.Validators
{
    public class CategoryCreateDTOValidator : AbstractValidator<CategoryCreateDTO>
    {
        public CategoryCreateDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty")
                .MaximumLength(80)
                .WithMessage("Title must be at most 80 characters");
        }
    }
}
