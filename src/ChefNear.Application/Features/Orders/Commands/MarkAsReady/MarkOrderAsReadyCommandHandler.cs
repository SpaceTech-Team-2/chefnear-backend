using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Enums;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.MarkAsReady;

public class MarkOrderAsReadyCommandHandler(
    IUnitOfWork unitOfWork,
    INotificationService notificationService) : IRequestHandler<MarkOrderAsReadyCommand, Result>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly INotificationService notificationService = notificationService;

    public async Task<Result> Handle(MarkOrderAsReadyCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetAsync(o => o.Id == request.OrderId && o.ChefId == request.Chef.Id);

        if (order == null)
            return Result.Failure(DomainErrors.Order.OrderNotFound);

        if (order.Status != OrderStatus.Preparing)
            return Result.Failure(DomainErrors.Order.OrderMustBePreparing);

        order.MarkAsReady(request.DeliveryFee, request.EstimatedDeliveryTime);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var notification = new Notification
        {
            Title = "Order Delivery",
            Message = "Your order is ready and waiting for delivery!",
            Status = NotificationStatus.Pending,
            OrderId = order.Id,
            Type = NotificationType.OrderReady,
            UserId = order.ClientId
        };

        await unitOfWork.Notifications.AddAsync(notification);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await notificationService.SendAsync(
            notification.UserId,
            notification.Id,
            notification.Title,
            notification.Message,
            notification.Type
        );

        return Result.Success();
    }
}
