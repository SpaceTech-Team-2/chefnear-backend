using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ChefNear.Application.Features.DishImages.Commands.AddDishImage
{
    public class AddDishImageCommand : IRequest<Result<Guid>>
    {
        public Guid DishId { get; set; }

        public string ChefId { get; set; } = string.Empty;

        public IFormFile File { get; set; } = null!;

        public bool IsPrimary { get; set; }
    }
}