using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Dish.Queries.GetDishByIdQuery
{
    public class GetDishByIdQuery : IRequest<Result<DishDetailDto?>>
    {
        public Guid DishId { get; set; }
    }
}
