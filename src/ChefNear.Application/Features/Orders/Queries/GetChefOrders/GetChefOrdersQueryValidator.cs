using FluentValidation;

namespace ChefNear.Application.Features.Orders.Queries.GetChefOrders;

public class GetChefOrdersQueryValidator : AbstractValidator<GetChefOrdersQuery>
{
    public GetChefOrdersQueryValidator()
    {
        RuleFor(q => q.PageSize)
            .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
            .LessThanOrEqualTo(30).WithMessage("PageSize must be less than or equal to 30.");

        RuleFor(q => q.PageNumber)
            .GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
    }
}
