namespace ChefNear.Domain.Entities
{
    public class OrderItem
    {
        public Guid OrderId { get; set; }
        public Guid DishId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } = decimal.Zero;

        public Order Order { get; set; } = default!;
        public Dish Dish { get; set; } = default!;
    }
}
