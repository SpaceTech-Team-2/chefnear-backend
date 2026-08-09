using ChefNear.Shared.ResultPattern;
using MediatR;
using System.Text.Json.Serialization;

namespace ChefNear.Application.Features.Dishes.Commands;

public class DeleteDishCommand : IRequest<Result>
{
    public Guid DishId { get; set; }
    [JsonIgnore]
    public string ChefId { get; set; }=string.Empty;
}