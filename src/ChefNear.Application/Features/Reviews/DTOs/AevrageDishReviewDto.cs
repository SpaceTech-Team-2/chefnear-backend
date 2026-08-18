using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Reviews.DTOs
{
    public class AverageDishReviewDto
    {
        public Guid DishId { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
    }
}
