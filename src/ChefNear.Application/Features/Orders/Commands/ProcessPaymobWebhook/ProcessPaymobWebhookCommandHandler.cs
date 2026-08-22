using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Enums;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChefNear.Application.Features.Orders.Commands.ProcessPaymobWebhook;

public class ProcessPaymobWebhookCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<ProcessPaymobWebhookCommandHandler> logger,
    INotificationService notificationService) : IRequestHandler<ProcessPaymobWebhookCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ProcessPaymobWebhookCommandHandler> _logger = logger;
    private readonly INotificationService notificationService = notificationService;

    public async Task<Result> Handle(ProcessPaymobWebhookCommand request, CancellationToken cancellationToken)
    {
        var transaction = request.Webhook.Transaction;
        var merchantOrderId = transaction.Order.MerchantOrderId;

        // Correlate: special_reference (PaymentId) -> merchant_order_id
        if (!Guid.TryParse(merchantOrderId, out var paymentId))
        {
            _logger.LogWarning(
                "Paymob webhook received with invalid MerchantOrderId. MerchantOrderId: {MerchantOrderId}, TransactionId: {TransactionId}",
                merchantOrderId,
                transaction.TransactionId);

            return Result.Failure(DomainErrors.Payment.PaymentNotFound);
        }

        var payment = await _unitOfWork.Payments.GetByIdAsync(paymentId);

        if (payment is null)
        {
            _logger.LogWarning(
                "Payment not found for Paymob webhook. PaymentId: {PaymentId}, TransactionId: {TransactionId}",
                paymentId,
                transaction.TransactionId);

            return Result.Failure(DomainErrors.Payment.PaymentNotFound);
        }

        var order = await _unitOfWork.Orders.GetByIdAsync(payment.OrderId);

        if (order is null)
        {
            _logger.LogError(
                "Order not found for payment. PaymentId: {PaymentId}, OrderId: {OrderId}, TransactionId: {TransactionId}",
                paymentId,
                payment.OrderId,
                transaction.TransactionId);

            return Result.Failure(DomainErrors.Order.OrderNotFound);
        }

        // Handle Refund Callback
        if (transaction.IsRefunded)
        {
            if (payment.Status == PaymentStatus.Refunded)
            {
                _logger.LogInformation(
                    "Paymob webhook received for already-refunded payment. PaymentId: {PaymentId}, Status: {Status}, TransactionId: {TransactionId}",
                    paymentId,
                    payment.Status,
                    transaction.TransactionId);

                return Result.Success();
            }

            payment.ConfirmRefund();

            _logger.LogInformation(
                "Payment status updated to Refunded via Paymob webhook. PaymentId: {PaymentId}, OrderId: {OrderId}, TransactionId: {TransactionId}",
                paymentId,
                order.Id,
                transaction.TransactionId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        // Idempotency: if already processed for payment holding/failure/refund, return success without re-processing
        if (payment.Status == PaymentStatus.Held || payment.Status == PaymentStatus.Failed || payment.Status == PaymentStatus.Refunded)
        {
            _logger.LogInformation(
                "Paymob webhook received for already-processed payment. PaymentId: {PaymentId}, Status: {Status}, TransactionId: {TransactionId}",
                paymentId,
                payment.Status,
                transaction.TransactionId);

            return Result.Success();
        }

        payment.GatewayTransactionId = transaction.TransactionId.ToString();

        if (transaction.Success)
        {
            payment.Hold();
            order.Confirm();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Payment held and order confirmed. PaymentId: {PaymentId}, OrderId: {OrderId}, TransactionId: {TransactionId}",
                paymentId,
                order.Id,
                transaction.TransactionId);

            var notification = new Notification
            {
                Title = "New Order",
                Message = "You have received a new order!",
                Status = NotificationStatus.Pending,
                OrderId = order.Id,
                Type = NotificationType.OrderPlaced,
                UserId = order.ChefId
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await notificationService.SendAsync(
                notification.UserId,
                notification.Id,
                notification.Title,
                notification.Message,
                notification.Type
            );
        }
        else
        {
            var failureReason = transaction.DataMessage
                ?? transaction.TxnResponseCode
                ?? "Unknown failure reason";

            payment.MarkAsFailed(failureReason);
            order.SoftDelete();

            _logger.LogWarning(
                "Payment failed. PaymentId: {PaymentId}, OrderId: {OrderId}, TransactionId: {TransactionId}, Reason: {Reason}",
                paymentId,
                order.Id,
                transaction.TransactionId,
                failureReason);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}
