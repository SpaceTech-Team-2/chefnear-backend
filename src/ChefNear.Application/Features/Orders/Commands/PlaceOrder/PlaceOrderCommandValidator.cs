using FluentValidation;

namespace ChefNear.Application.Features.Orders.Commands.PlaceOrder;

public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Order must contain at least one item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.DishId)
                .NotEmpty().WithMessage("DishId is required.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        });

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");

        RuleFor(x => x.DeliveryAddressId)
            .NotNull().When(x => x.DeliveryAddress == null).WithMessage("Delivery address is required.");

        RuleFor(x => x.DeliveryAddress)
            .NotNull().When(x => x.DeliveryAddressId == null).WithMessage("Delivery address is required.");

        RuleFor(x => x.DeliveryAddress)
            .ChildRules(d => 
            {
                d.RuleFor(x => x.City)
                    .NotEmpty().WithMessage("City is required.");

                d.RuleFor(x => x.Latitude)
                    .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

                d.RuleFor(x => x.Longitude)
                    .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
            }).When(x => x.DeliveryAddress != null);
    }
}
