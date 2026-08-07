using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Dish.DTOs
{

    public class DishSummaryDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? PrimaryImageUrl { get; set; }

        public string ChefDisplayName { get; set; } = string.Empty;

        public double DistanceKm { get; set; }
    }
}
