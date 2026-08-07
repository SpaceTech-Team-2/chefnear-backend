using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.DishImages.Commands.SetPrimaryDishImage
{
    public class SetPrimaryDishImageCommand : IRequest<Result>
    {
        public Guid ImageId { get; set; }

        public string ChefId { get; set; } = string.Empty;
    }
}