using Microsoft.AspNetCore.Identity;
using ChefNear.Domain.Common;
using HomeChefMarketplace.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ChefNear.Domain.Entities
{
    public class User : IdentityUser 
    {
       

        public UserRole Role { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;

        public Guid? KitchenAddressId { get; set; }
        public Address? KitchenAddress { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Description { get; set; }
        public double? ReliabilityScore { get; set; }

        // ---- Navigation ----
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Dispute> ResolvedDisputes { get; set; } = new List<Dispute>();

        public ICollection<Dispute> FiledDisputes { get; set; } = new List<Dispute>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}