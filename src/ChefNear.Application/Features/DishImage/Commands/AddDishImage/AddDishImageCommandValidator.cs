using FluentValidation;

namespace ChefNear.Application.Features.DishImages.Commands.AddDishImage
{
    public class AddDishImageCommandValidator : AbstractValidator<AddDishImageCommand>
    {
        public AddDishImageCommandValidator()
        {
            RuleFor(x => x.DishId)
                .NotEmpty();

            RuleFor(x => x.ChefId)
                .NotEmpty();

            RuleFor(x => x.ImageUrl)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}