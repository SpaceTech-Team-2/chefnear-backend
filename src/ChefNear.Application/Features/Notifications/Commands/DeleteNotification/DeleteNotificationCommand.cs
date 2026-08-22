using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Notifications.Commands.DeleteNotification;

public record DeleteNotificationCommand(Guid Id, string UserId) : IRequest<Result>;
