namespace ChefNear.Application.Common.Payments;

public class OrderSummary
{
    public Guid OrderId { get; set; }
    public Guid PaymentId { get; set; }
    public decimal TotalAmount { get; set; }    
    public string DishName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string ClientFirstName { get; set; } = string.Empty;
    public string ClientLastName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public string ClientPhone { get; set; } = string.Empty;
}
