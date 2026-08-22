using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Notifications.Commands.ClearAllNotifications;

public record ClearAllNotificationsCommand(string UserId) : IRequest<Result>;
