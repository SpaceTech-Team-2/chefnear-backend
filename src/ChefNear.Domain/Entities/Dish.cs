using ChefNear.Domain.Common;
using HomeChefMarketplace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Domain.Entities
{
   
    public class Dish : BaseEntity, ISoftDelete
    {
        public string ChefId { get; set; } = string.Empty;
        public Chef Chef { get; set; } = null!;

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public string? AllergenInfo { get; set; }       
        public DishStatus Status { get; set; } = DishStatus.Available;

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<DishImage> Images { get; set; } = new List<DishImage>();
        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }

}
