using ChefNear.Application.Features.Auth.DTOs;
using ChefNear.Application.Model;
using ChefNear.Domain.Enums;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System.Text.Json.Serialization;

namespace ChefNear.Application.Features.Orders.Commands.PlaceOrder;

public record OrderItemRequest(Guid DishId, int Quantity);

public record PlaceOrderCommand( 
    Guid IdempotencyKey,
    List<OrderItemRequest> Items,
    string Notes,
    Guid? DeliveryAddressId,
    AddressDto? DeliveryAddress,
    PaymentGateway PaymentGateway
) : IRequest<Result<PlaceOrderResponse>>
{
    [JsonIgnore]
    public CurrentUser Client { get; set; } = default!;
}
