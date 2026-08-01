using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Dish.DTOs
{

    public class IngredientDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Quantity { get; set; }
    }
}
