using ChefNear.Application.Features.Dish.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Dish.Queries.GetDishByIdQuery
{
    public class GetDishByIdQuery : IRequest<DishDetailDto?>
    {
        public Guid DishId { get; set; }
    }
}
