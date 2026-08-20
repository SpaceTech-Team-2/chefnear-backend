using ChefNear.Application.Common.Jobs;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Enums;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using Hangfire;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.MarkAsDelivered;

public class MarkAsDeliveredCommandHandler(
    IUnitOfWork unitOfWork,
    IBackgroundJobClient backgroundJobClient,
    IAddChefEarningsJob addChefEarningsJob,
    INotificationService notificationService) : IRequestHandler<MarkAsDeliveredCommand, Result>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IBackgroundJobClient backgroundJobClient = backgroundJobClient;
    private readonly IAddChefEarningsJob addChefEarningsJob = addChefEarningsJob;
    private readonly INotificationService notificationService = notificationService;

    public async Task<Result> Handle(MarkAsDeliveredCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders
            .GetAsync(o => o.Id == request.OrderId && o.ClientId == request.Client.Id, nameof(Order.Payment));

        if (order == null)
            return Result.Failure(DomainErrors.Order.OrderNotFound);

        if (order.Status != OrderStatus.OutForDelivery)
            return Result.Failure(DomainErrors.Order.OrderMustBeReady);

        var payment = order.Payment;

        if (payment == null)
            return Result.Failure(Error.Failure("OrderNotPaid", "This order not paid yet to mark it as Delivered."));

        order.MarkAsDelivered();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        backgroundJobClient
            .Schedule(() => addChefEarningsJob.ExecuteAsync(payment.Id, order.ChefId), TimeSpan.FromSeconds(10));

        var notification = new Notification
        {
            Message = "Your order has been delivered to the client!",
            Status = NotificationStatus.Pending,
            OrderId = order.Id,
            Type = NotificationType.OrderDelivered,
            UserId = order.ChefId
        };

        await unitOfWork.Notifications.AddAsync(notification);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Send In-App Notification
        await notificationService.SendAsync(
            notification.UserId,
            notification.Id,
            notification.Message,
            notification.Type
        );

        return Result.Success();
    }
}
