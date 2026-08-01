using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Dish.DTOs
{

    public class DishDetailDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int QuantityAvailable { get; set; }

        public string? AllergenInfo { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string ChefId { get; set; } = string.Empty;

        public string ChefDisplayName { get; set; } = string.Empty;

        public double ChefReliabilityScore { get; set; }

        public List<DishImageDto> Images { get; set; } = new();

        public List<IngredientDtos> Ingredients { get; set; } = new();
    }

    public class DishImageDto
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }

        public int DisplayOrder { get; set; }
    }

    public class IngredientDtos
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Quantity { get; set; }
    }
}
