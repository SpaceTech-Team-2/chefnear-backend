using AutoMapper;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Notifications.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChefNear.Application.Features.Notifications.Queries.GetNotifications;

public class GetNotificationsQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetNotificationsQuery, Result<List<NotificationDto>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<NotificationDto>>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var notifications = await _unitOfWork.Notifications.GetUserNotificationsPaginatedAsync(
            request.User.Id,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var result = _mapper.Map<List<NotificationDto>>(notifications);
        return Result.Success(result);
    }
}
