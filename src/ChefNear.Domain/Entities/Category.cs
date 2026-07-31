using ChefNear.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Domain.Entities
{
   
  
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    }

}
