using AutoMapper;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Orders.DTOs;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Errors;
using ChefNear.Shared.Constants;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetOrderByIdQuery, Result<GetOrderByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<GetOrderByIdDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetByIdWithTrackingAsync(request.OrderId);

        if (order == null ||
            request.User.IsInRole(UserRoles.Client) && request.User.Id != order.ClientId ||
            request.User.IsInRole(UserRoles.Chef) && request.User.Id != order.ChefId )
            return DomainErrors.Order.OrderNotFound;

        var dto = _mapper.Map<GetOrderByIdDto>(order);
        dto.Tracking = GetTrackingForOrder(order);

        return Result.Success(dto);
    }

    private List<OrderTrackingDto> GetTrackingForOrder(Order order)
    {
        return new List<OrderTrackingDto>
        {
            new OrderTrackingDto
            {
                Status = OrderStatus.Confirmed,
                Timestamp = order.ConfirmedAt,
                Completed = order.ConfirmedAt.HasValue
            },
            new OrderTrackingDto
            {
                Status = OrderStatus.Accepted,
                Timestamp = order.AcceptedAt,
                Completed = order.AcceptedAt.HasValue
            },
            new OrderTrackingDto
            {
                Status = OrderStatus.Preparing,
                Timestamp = order.StartPreparingAt,
                Completed = order.StartPreparingAt.HasValue
            },
            new OrderTrackingDto
            {
                Status = OrderStatus.OutForDelivery,
                Timestamp = order.ReadyAt,
                Completed = order.ReadyAt.HasValue
            },
            new OrderTrackingDto
            {
                Status = OrderStatus.Delivered,
                Timestamp = order.DeliveredAt,
                Completed = order.DeliveredAt.HasValue
            },
            new OrderTrackingDto
            {
                Status = OrderStatus.Cancelled,
                Timestamp = order.CanceledAt,
                Completed = order.CanceledAt.HasValue
            }
        };
    }
}
