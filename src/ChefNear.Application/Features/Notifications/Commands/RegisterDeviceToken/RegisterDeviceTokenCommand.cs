using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Notifications.Commands.RegisterDeviceToken;

public record RegisterDeviceTokenCommand(
    string Token,
    string UserId   
) : IRequest<Result>;
