using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Ingredints.Commands.UpdateIngredient
{
    public class UpdateIngredientCommand : IRequest<Result>
    {
        public Guid IngredientId { get; set; }
        public string ChefId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Quantity { get; set; }
    }
}
