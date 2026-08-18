using Asp.Versioning;
using ChefNear.Application.Features.Reviews.Commands.AddReview;
using ChefNear.Application.Features.Reviews.Commands.UpdateReview;
using ChefNear.Application.Features.Reviews.Queries.GetChefRating;
using ChefNear.Application.Features.Reviews.Queries.GetReviewByDishAndOrderId;
using ChefNear.Application.Features.Reviews.Queries.getReviewByDishId;
using ChefNear.Application.Features.Reviews.Queries.GetReviewByDishId;
using ChefNear.Application.Features.Reviews.Queries.GetReviewById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class ReviewController : BaseApiController
    {
        private readonly IMediator mediator;

        public ReviewController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("chef/{chefId}/rating")]
        public async Task<IActionResult> GetChefReview(string chefId)
        {

            var res =await mediator.Send(new GetChefRatingQuery { ChefId = chefId });
            return HandleResult(res);
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Create(CreateReviewCommand command)
        {
            var clientId = GetUser().Id;
            command.ClientId = clientId;
            var res = await mediator.Send(command);
            return HandleResult(res, "Review Created successfully.");
        }
        [HttpPut("{reviewId:guid}")]

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Update(Guid reviewId, UpdateReviewCommand command)
        {
            command.ReviewId = reviewId;
            var clientId = GetUser().Id;
            command.ClientId = clientId;
            var res = await mediator.Send(command);
            return HandleResult(res, "Review updated successfully.");
        }
        [HttpGet("dish/{dishId:guid}/rating")]
        public async Task<IActionResult> GetAverageDishRating(Guid dishId)
        {
            var result = await Mediator.Send(
                new GetAverageReviewByDishIdQuery
                {
                    DishId = dishId
                });

            return HandleResult(result);
        }

        [HttpGet("{reviewId:guid}")]
        public async Task<IActionResult> GetById(Guid reviewId)
        {
            var res = await mediator.Send(
                new GetReviewByIdQuery
                {
                    ReviewId = reviewId
                });

            return HandleResult(res);
        }


        [HttpGet("dish/{dishId:guid}")]
        public async Task<IActionResult> GetByDishId(
        Guid dishId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            var result = await Mediator.Send(
                new GetReviewByDishIdQuery
                {
                    DishId = dishId,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                });

            return HandleResult(result);
        }

        [HttpGet("dish/{dishId:guid}/order/{orderId:guid}")]
        public async Task<IActionResult> GetByDishAndOrder(Guid dishId,Guid orderId)
        {
            var result = await Mediator.Send(
                new GetReviewByDishAndOrderIdQuery
                {
                    DishId = dishId,
                    OrderId = orderId
                });

            return HandleResult(result);
        }
    }
}
