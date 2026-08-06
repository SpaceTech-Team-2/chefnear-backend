using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChefNear.Application.Features.Orders.Commands.ProcessPaymobWebhook;

public class ProcessPaymobWebhookCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<ProcessPaymobWebhookCommandHandler> logger) : IRequestHandler<ProcessPaymobWebhookCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ProcessPaymobWebhookCommandHandler> _logger = logger;

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

            payment.Status = PaymentStatus.Refunded;
            payment.RefundedAt = DateTime.UtcNow;

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
            payment.Status = PaymentStatus.Held;
            payment.HeldAt = DateTime.UtcNow;

            order.Status = OrderStatus.Confirmed;

            _logger.LogInformation(
                "Payment held and order confirmed. PaymentId: {PaymentId}, OrderId: {OrderId}, TransactionId: {TransactionId}",
                paymentId,
                order.Id,
                transaction.TransactionId);
        }
        else
        {
            var failureReason = transaction.DataMessage
                ?? transaction.TxnResponseCode
                ?? "Unknown failure reason";

            payment.Status = PaymentStatus.Failed;
            payment.FailureReason = failureReason;

            order.IsDeleted = true;
            order.DeletedAt = DateTime.UtcNow;

            _logger.LogWarning(
                "Payment failed. PaymentId: {PaymentId}, OrderId: {OrderId}, TransactionId: {TransactionId}, Reason: {Reason}",
                paymentId,
                order.Id,
                transaction.TransactionId,
                failureReason);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
