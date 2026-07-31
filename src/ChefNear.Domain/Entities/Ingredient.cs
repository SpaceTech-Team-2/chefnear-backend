using ChefNear.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Domain.Entities
{
   
    public class Ingredient : BaseEntity
    {
        public Guid DishId { get; set; }
        public Dish Dish { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public string? Quantity { get; set; }  
    }

}
