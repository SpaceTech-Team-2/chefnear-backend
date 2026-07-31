using ChefNear.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Domain.Entities
{
  
    public class DishImage : BaseEntity
    {
        public Guid DishId { get; set; }
        public Dish Dish { get; set; } = null!;

        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }

}
