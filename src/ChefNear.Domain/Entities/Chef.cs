using System;
using System.Collections.Generic;

namespace ChefNear.Domain.Entities
{
    public class Chef : User
    {
        public Guid? KitchenAddressId { get; set; }
        public Address? KitchenAddress { get; set; }

        public string? Description { get; set; }
        public double? ReliabilityScore { get; set; }

        // Navigation
        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
        public Wallet? Wallet { get; set; }
    }
}
