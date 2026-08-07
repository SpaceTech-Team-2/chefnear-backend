using ChefNear.Application.Model;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.StartPreparing;

public record StartOrderPreparingRequest(TimeSpan? EstimatedCookingTime);

public record StartOrderPreparingCommand(
    Guid OrderId,
    CurrentUser Chef,
    TimeSpan? EstimatedCookingTime
) : IRequest<Result>;
    