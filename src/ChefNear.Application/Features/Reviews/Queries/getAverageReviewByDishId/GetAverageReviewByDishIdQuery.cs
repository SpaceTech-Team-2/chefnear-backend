using ChefNear.Application.Features.Reviews.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Reviews.Queries.getReviewByDishId
{
    public class GetAverageReviewByDishIdQuery : IRequest<Result<AverageDishReviewDto>>
    {
         public Guid DishId { get; set; }

    }
}
