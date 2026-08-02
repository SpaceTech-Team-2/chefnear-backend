using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.DishImage.Queries.GetDishImages
{
    public class GetDishImagesQuery : IRequest<Result<List<DishImageDto>>>
    {
        public Guid DishId { get; set; }
    }
}
