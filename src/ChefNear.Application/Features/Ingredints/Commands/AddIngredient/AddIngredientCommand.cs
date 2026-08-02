using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Ingredints.Commands.AddIngredient
{
    public class AddIngredientCommand : IRequest<Result<Guid>>
    {
        public Guid DishId { get; set; }
        public string ChefId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Quantity { get; set; }
    }
}
