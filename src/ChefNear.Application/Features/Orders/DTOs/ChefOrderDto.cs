using ChefNear.Application.Features.Address.DTOs;
using HomeChefMarketplace.Domain.Enums;

namespace ChefNear.Application.Features.Orders.DTOs;

public class ChefOrderDto
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty!;

    public string ClientId { get; set; } = string.Empty!;
    public string ClientName { get; set; } = string.Empty!;

    public AddressDto Address { get; set; } = default!;
    public List<OrderItemDto> Items { get; set; } = new();
}
