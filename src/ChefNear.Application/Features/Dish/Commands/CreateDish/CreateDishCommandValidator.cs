using FluentValidation;

namespace ChefNear.Application.Features.Dishes.Commands;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.QuantityAvailable)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}