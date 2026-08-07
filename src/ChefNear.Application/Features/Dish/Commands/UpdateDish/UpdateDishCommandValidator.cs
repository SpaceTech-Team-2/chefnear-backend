using FluentValidation;

namespace ChefNear.Application.Features.Dishes.Commands;

public class UpdateDishCommandValidator : AbstractValidator<UpdateDishCommand>
{
    public UpdateDishCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.QuantityAvailable)
            .GreaterThanOrEqualTo(0);
    }
}