using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Reviews.DTOs
{
    public class ChefRatingDto
    {
        public string ChefId { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}
