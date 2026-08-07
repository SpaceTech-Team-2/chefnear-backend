using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.DishImages.Commands.RemoveDishImage
{
    public class RemoveDishImageCommand : IRequest<Result>
    {
        public Guid ImageId { get; set; }

        public string ChefId { get; set; } = string.Empty;
    }
}