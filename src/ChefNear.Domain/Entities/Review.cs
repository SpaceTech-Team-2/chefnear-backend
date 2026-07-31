using ChefNear.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Domain.Entities
{
  
    public class Review : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid DishId { get; set; }
        public Dish Dish { get; set; } = null!;

        public string ClientId { get; set; } = string.Empty;  
        public User Client { get; set; } = null!;

        public int Rating { get; set; }           
        public string? Comment { get; set; }
    }

}
