using ChefNear.Application.Model;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.AcceptOrder;

public record AcceptOrderCommand(
    Guid OrderId,
    CurrentUser Chef
) : IRequest<Result>;
