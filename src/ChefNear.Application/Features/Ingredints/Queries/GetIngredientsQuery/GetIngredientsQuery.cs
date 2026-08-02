using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Ingredints.Queries.GetIngredientsQuery
{
    public class GetIngredientsQuery : IRequest<Result<List<IngredientDtos>>>
    {
        public Guid DishId { get; set; }
    }
}
