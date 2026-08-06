using ChefNear.Application.Common.Payments;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Enums;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChefNear.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler(
    IUnitOfWork unitOfWork,
    IPaymentGatewayFactory paymentGatewayFactory,
    ILogger<CancelOrderCommandHandler> logger) : IRequestHandler<CancelOrderCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPaymentGatewayFactory _paymentGatewayFactory = paymentGatewayFactory;
    private readonly ILogger<CancelOrderCommandHandler> _logger = logger;

    public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(request.OrderId);

        if (order is null)
            return Result.Failure(DomainErrors.Order.OrderNotFound);

        if (order.Status == OrderStatus.Cancelled)
            return Result.Failure(DomainErrors.Order.OrderAlreadyCancelled);

        var currentUserId = request.User.Id;
        var isClient = order.ClientId == currentUserId;
        var isChef = order.OrderItems.Any(item => item.Dish != null && item.Dish.ChefId == currentUserId);

        if (!isClient && !isChef)
            return Result.Failure(DomainErrors.Order.UnauthorizedCancellation);

        if (isClient)
        {
            if (!IsClientReason(request.ReasonType))
                return Result.Failure(DomainErrors.Order.InvalidCancellationReason);

            // Can only cancel if order status (Accepted | Confirmed)
            if (order.Status != OrderStatus.Pending &&
                order.Status != OrderStatus.Accepted &&
                order.Status != OrderStatus.Confirmed)
            {
                return Result.Failure(DomainErrors.Order.CancellationNotAllowed);
            }

            var cancelledBy = isClient ? CancelledBy.Client : CancelledBy.Chef;
            order.Cancel(cancelledBy, request.ReasonType, request.ReasonFreeText);

            // Payment Refund Handling
            if (order.Payment != null)
            {
                if (order.Payment.Status == PaymentStatus.Held)
                {
                    try
                    {
                        var paymentGateway = _paymentGatewayFactory.GetGateway(PaymentGateway.Paymob);
                        var refundTransactionId = await paymentGateway.RefundAsync(order.Payment.GatewayTransactionId!, order.Payment.Amount);
                        order.Payment.Refund(refundTransactionId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred while calling RefundAsync for OrderId: {OrderId}", order.Id);
                    }
                }
                else if (order.Payment.Status == PaymentStatus.Pending)
                {
                    order.Payment.MarkAsFailed("Order cancelled before payment completed.");
                }
            }

            // TODO: Notification creation

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Order #{OrderId} successfully cancelled by User {UserId}", order.Id, currentUserId);
        }
        
        return Result.Success();
    }
    

    private static bool IsClientReason(CancellationReasonType reasonType) =>
        reasonType is CancellationReasonType.ClientChangedMind
            or CancellationReasonType.ClientOrderDelayed
            or CancellationReasonType.ClientIncorrectDetails
            or CancellationReasonType.ClientOther;

    private static bool IsChefReason(CancellationReasonType reasonType) =>
        reasonType is CancellationReasonType.ChefOutofIngredients
            or CancellationReasonType.ChefKitchenBusy
            or CancellationReasonType.ChefPersonalEmergency
            or CancellationReasonType.ChefOther;
}
