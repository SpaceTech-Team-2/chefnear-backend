using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Reviews.DTOs
{
    public class ReviewDto
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid DishId { get; set; }

        public string ClientId { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;

        public int Rating { get; set; }

        public string? Comment { get; set; }
    }
}
