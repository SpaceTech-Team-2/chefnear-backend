using ChefNear.Application.Features.Orders.DTOs;
using ChefNear.Application.Model;
using ChefNear.Domain.Entities;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(
    Guid OrderId,
    CurrentUser User
) : IRequest<Result<GetOrderByIdDto>>;
