using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Ingredints.Commands.AddIngredient
{
    using FluentValidation;


    public class AddIngredientCommandValidator : AbstractValidator<AddIngredientCommand>
    {
        public AddIngredientCommandValidator()
        {
            RuleFor(x => x.DishId)
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
