using ChefNear.Application.Features.Reviews.Queries.getReviewByDishId;
using FluentValidation;

namespace ChefNear.Application.Features.Reviews.Queries.GetReviewByDishId
{
    public class GetAvwrageReviewByDishIdQueryValidator: AbstractValidator<GetAverageReviewByDishIdQuery>
    {
        public GetAvwrageReviewByDishIdQueryValidator()
        {
            RuleFor(x => x.DishId)
                .NotEmpty()
                .WithMessage("DishId is required.");
        }
    }
}