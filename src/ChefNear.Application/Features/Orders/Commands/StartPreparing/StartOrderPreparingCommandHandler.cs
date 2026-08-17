using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Enums;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.StartPreparing;

public class StartOrderPreparingCommandHandler(
    IUnitOfWork unitOfWork,
    INotificationService notificationService) : IRequestHandler<StartOrderPreparingCommand, Result>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly INotificationService notificationService = notificationService;

    public async Task<Result> Handle(StartOrderPreparingCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetAsync(o => o.Id == request.OrderId && o.ChefId == request.Chef.Id);

        if (order == null)
            return Result.Failure(DomainErrors.Order.OrderNotFound);

        if (order.Status != OrderStatus.Accepted)
            return Result.Failure(DomainErrors.Order.OrderMustBeAccepted);

        order.StartPreparing(request.EstimatedCookingTime);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var notification = new Notification
        {
            Message = "Your order is being prepared!",
            Status = NotificationStatus.Pending,
            OrderId = order.Id,
            Type = NotificationType.OrderPreparing,
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
