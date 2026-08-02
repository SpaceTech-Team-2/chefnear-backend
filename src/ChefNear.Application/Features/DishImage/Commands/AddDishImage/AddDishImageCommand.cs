using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.DishImages.Commands.AddDishImage
{
    public class AddDishImageCommand : IRequest<Result<Guid>>
    {
        public Guid DishId { get; set; }

        public string ChefId { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }
    }
}