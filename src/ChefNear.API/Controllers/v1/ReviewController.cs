using Asp.Versioning;
using ChefNear.Application.Features.Reviews.Commands.AddReview;
using ChefNear.Application.Features.Reviews.Commands.UpdateReview;
using ChefNear.Application.Features.Reviews.DTOs;
using ChefNear.Application.Features.Reviews.Queries.GetChefRating;
using ChefNear.Application.Features.Reviews.Queries.GetReviewByDishAndOrderId;
using ChefNear.Application.Features.Reviews.Queries.getReviewByDishId;
using ChefNear.Application.Features.Reviews.Queries.GetReviewByDishId;
using ChefNear.Application.Features.Reviews.Queries.GetReviewById;
using ChefNear.Shared.Constants;
using ChefNear.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

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
        [ProducesResponseType<ApiResponse<ChefRatingDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetChefReview(string chefId)
        {

            var res =await mediator.Send(new GetChefRatingQuery { ChefId = chefId });
            return HandleResult(res);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Client)]
        [ProducesResponseType<ApiResponse<Guid>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateReviewCommand command)
        {
            var clientId = GetUser().Id;
            command.ClientId = clientId;

            var res = await mediator.Send(command);
            return HandleResult(res, "Review Created successfully.");
        }

        [HttpPut("{reviewId:guid}")]
        [Authorize(Roles = "Client")]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(Guid reviewId, UpdateReviewCommand command)
        {
            command.ReviewId = reviewId;
            var clientId = GetUser().Id;
            command.ClientId = clientId;
            var res = await mediator.Send(command);
            return HandleResult(res, "Review updated successfully.");
        }

        [HttpGet("dish/{dishId:guid}/rating")]
        [ProducesResponseType<ApiResponse<AverageDishReviewDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType<ApiResponse<ReviewDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType<ApiResponse<PagedResult<ReviewDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType<ApiResponse<ReviewDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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
