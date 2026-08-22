using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Notifications.Commands.ClearAllNotifications;

public class ClearAllNotificationsCommandHandler : IRequestHandler<ClearAllNotificationsCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public ClearAllNotificationsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ClearAllNotificationsCommand request, CancellationToken cancellationToken)
    {
        var notifications = await _unitOfWork.Notifications.FindAsync(n => n.UserId == request.UserId);

        foreach (var notification in notifications)
        {
            await _unitOfWork.Notifications.DeleteAsync(notification);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
