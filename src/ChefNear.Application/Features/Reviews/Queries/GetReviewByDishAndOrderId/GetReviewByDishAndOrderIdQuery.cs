using ChefNear.Application.Features.Reviews.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Reviews.Queries.GetReviewByDishAndOrderId
{
    public class GetReviewByDishAndOrderIdQuery
    : IRequest<Result<ReviewDto>>
    {
        public Guid DishId { get; set; }

        public Guid OrderId { get; set; }
    }
}
