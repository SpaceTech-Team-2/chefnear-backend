using ChefNear.Application.Features.Reviews.Commands.AddReview;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.DishId).NotEmpty();

            RuleFor(x => x.Rating).InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.");
            RuleFor(x => x.Comment).MaximumLength(1000);
        }
    }
}
