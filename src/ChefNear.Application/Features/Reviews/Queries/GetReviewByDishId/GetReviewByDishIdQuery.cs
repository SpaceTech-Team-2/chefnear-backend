using ChefNear.Application.Features.Reviews.DTOs;
using ChefNear.Shared.Responses.ChefNear.Shared.Responses;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Reviews.Queries.GetReviewByDishId
{
    public class GetReviewByDishIdQuery
     : IRequest<Result<PagedResult<ReviewDto>>>
    {
        public Guid DishId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
