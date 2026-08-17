using ChefNear.Application.Features.Notifications.DTOs;
using ChefNear.Application.Model;
using ChefNear.Shared.ResultPattern;
using MediatR;

using System.Collections.Generic;

namespace ChefNear.Application.Features.Notifications.Queries.GetNotifications;

public record GetNotificationsRequest(
    int PageNumber = 1,
    int PageSize = 10
);

public record GetNotificationsQuery(
    CurrentUser User,
    int PageNumber,
    int PageSize
) : IRequest<Result<List<NotificationDto>>>;