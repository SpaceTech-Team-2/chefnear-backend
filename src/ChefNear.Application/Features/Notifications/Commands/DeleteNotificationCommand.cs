using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChefNear.Application.Features.Notifications.Commands
{
    public record DeleteNotificationCommand(Guid Id, string UserId) : IRequest<Result>;

    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNotificationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(request.Id);

            if (notification is null || notification.UserId != request.UserId)
            {
                return Result.Failure(DomainErrors.Notifications.NotFound);
            }

            await _unitOfWork.Notifications.DeleteAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
