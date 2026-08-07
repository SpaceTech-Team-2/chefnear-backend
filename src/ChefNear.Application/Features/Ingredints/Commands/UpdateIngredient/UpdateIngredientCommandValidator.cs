using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Ingredints.Commands.UpdateIngredient
{
    
    public class UpdateIngredientCommandValidator : AbstractValidator<UpdateIngredientCommand>
    {
        public UpdateIngredientCommandValidator()
        {
            RuleFor(x => x.IngredientId)
                .NotEmpty();

            RuleFor(x => x.ChefId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Quantity)
                .MaximumLength(100);
        }
    }
}
