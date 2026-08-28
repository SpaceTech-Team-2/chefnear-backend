using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Admin.Queries.DTOs
{
    public class AdminReviewDto
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; }
        public string ChefName { get; set; }
        public Guid DishId { get; set; }
        public string DishName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
