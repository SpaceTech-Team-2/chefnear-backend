using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Enums;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.AcceptOrder;

public class AcceptOrderCommandHandler(
    IUnitOfWork unitOfWork,
    INotificationService notificationService
    ) : IRequestHandler<AcceptOrderCommand, Result>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly INotificationService notificationService = notificationService;

    public async Task<Result> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetAsync(o => o.Id == request.OrderId && o.ChefId == request.Chef.Id);

        if (order == null)
            return Result.Failure(DomainErrors.Order.OrderNotFound);

        if (order.Status != OrderStatus.Confirmed)
            return Result.Failure(DomainErrors.Order.OrderMustBeConfirmed);

        order.Accept();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var notification = new Notification
        {
            Message = "Your order has been accepted by the chef.",
            Status = NotificationStatus.Pending,
            OrderId = order.Id,
            Type = NotificationType.OrderAccepted,
            UserId = order.ClientId
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
