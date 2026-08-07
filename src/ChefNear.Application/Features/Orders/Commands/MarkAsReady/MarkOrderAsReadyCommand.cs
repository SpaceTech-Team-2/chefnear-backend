using ChefNear.Application.Model;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.MarkAsReady;

public record MarkOrderAsReadyRequest(TimeSpan? EstimatedDeliveryTime);

public record MarkOrderAsReadyCommand(
    Guid OrderId,
    CurrentUser Chef,
    TimeSpan? EstimatedDeliveryTime
) : IRequest<Result>;
