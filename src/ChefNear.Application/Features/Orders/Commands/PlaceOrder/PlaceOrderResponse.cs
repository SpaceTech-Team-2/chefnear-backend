namespace ChefNear.Application.Features.Orders.Commands.PlaceOrder;

public class PlaceOrderResponse
{
    public Guid OrderId { get; set; }
    public string ClientSecret { get; set; } = string.Empty;
    public string PublikKey { get; set; } = string.Empty;
}
