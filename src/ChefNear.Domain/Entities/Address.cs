using ChefNear.Domain.Common;
using System;
using System.Collections.Generic;

namespace ChefNear.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string? ClientId { get; set; } 
        public Client? Client { get; set; }

        public string? Label { get; set; } 
        public string City { get; set; } = string.Empty;
        public string? Details { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsDefault { get; set; }

        // Navigation
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}