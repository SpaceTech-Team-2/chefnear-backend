using ChefNear.Domain.Enums;
using FluentValidation;
using HomeChefMarketplace.Domain.Enums;

namespace ChefNear.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.");

        RuleFor(x => x.ReasonType)
            .IsInEnum().WithMessage("Invalid cancellation reason type.");

        RuleFor(x => x.ReasonFreeText)
            .MaximumLength(500).WithMessage("Custom comment cannot exceed 500 characters.");

        RuleFor(x => x.ReasonFreeText)
            .NotEmpty()
            .When(x => x.ReasonType == CancellationReasonType.ClientOther || x.ReasonType == CancellationReasonType.ChefOther)
            .WithMessage("Custom comment is required when reason is set to Other.");
    }
}
