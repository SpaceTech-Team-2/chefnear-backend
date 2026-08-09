using ChefNear.Shared.ResultPattern;
using MediatR;
using System.Text.Json.Serialization;

namespace ChefNear.Application.Features.DishImages.Commands.SetPrimaryDishImage
{
    public class SetPrimaryDishImageCommand : IRequest<Result>
    {
        public Guid ImageId { get; set; }
        [JsonIgnore]

        public string ChefId { get; set; } = string.Empty;
    }
}