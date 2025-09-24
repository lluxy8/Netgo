using FluentValidation;

namespace Netgo.Application.DTOs.Category.Validators
{
    public class CategoryUpdateDTOValidatior : AbstractValidator<CategoryUpdateDTO>
    {
        public CategoryUpdateDTOValidatior()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty")
                .MaximumLength(80)
                .WithMessage("Title must be at most 80 characters");
        }
    }
}
