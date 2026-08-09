using ChefNear.Shared.ResultPattern;
using MediatR;
using System.Text.Json.Serialization;

namespace ChefNear.Application.Features.DishImages.Commands.RemoveDishImage
{
    public class RemoveDishImageCommand : IRequest<Result>
    {
        public Guid ImageId { get; set; }
        [JsonIgnore]
        public string ChefId { get; set; } = string.Empty;
    }
}