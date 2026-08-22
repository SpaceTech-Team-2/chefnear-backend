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

namespace ChefNear.Application.Features.Orders.Commands.AcceptOrder;

public class AcceptOrderCommandHandler(
    IUnitOfWork unitOfWork,
    INotificationService notificationService
    ) : IRequestHandler<AcceptOrderCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly INotificationService _notificationService = notificationService;

    public async Task<Result> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetAsync(o => o.Id == request.OrderId && o.ChefId == request.Chef.Id);

        if (order == null)
            return Result.Failure(DomainErrors.Order.OrderNotFound);

        if (order.Status != OrderStatus.Confirmed)
            return Result.Failure(DomainErrors.Order.OrderMustBeConfirmed);

        order.Accept();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var notification = new Notification
        {
            Title = "Order Status Updated",
            Message = "Your order has been accepted by the chef.",
            Status = NotificationStatus.Pending,
            OrderId = order.Id,
            Type = NotificationType.OrderAccepted,
            UserId = order.ClientId
        };

        await _unitOfWork.Notifications.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _notificationService.SendAsync(
            notification.UserId,
            notification.Id,
            notification.Title,
            notification.Message,
            notification.Type
        );

        return Result.Success();
    }
}
