using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.DishImage.DTOs
{
    public class DishImageDto
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }

        public int DisplayOrder { get; set; }
    }
}
