using ChefNear.Application.Model;
using HomeChefMarketplace.Domain.Enums;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System.Text.Json.Serialization;
using ChefNear.Domain.Enums;

namespace ChefNear.Application.Features.Orders.Commands.CancelOrder;

public record CancelOrderCommand(
    Guid OrderId,
    CancellationReasonType ReasonType,
    string? CustomComment
) : IRequest<Result>
{
    [JsonIgnore]
    public CurrentUser User { get; set; } = default!;
}
