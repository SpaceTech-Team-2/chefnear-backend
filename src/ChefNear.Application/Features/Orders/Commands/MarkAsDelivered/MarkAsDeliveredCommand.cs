using ChefNear.Application.Model;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.MarkAsDelivered;

public record MarkAsDeliveredCommand(
    Guid OrderId,
    CurrentUser Client  
) : IRequest<Result>;
