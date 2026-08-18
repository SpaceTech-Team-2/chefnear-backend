using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Reviews.Commands.AddReview;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Enums;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateReviewCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetAsync(o => o.Id == request.OrderId,"OrderItems");
            if (order == null)
            {
                return Result.Failure<Guid>(
                    Error.NotFound("Order.NotFound", "Order not found."));
            }

            if (order.ClientId != request.ClientId)
            {
                return Result.Failure<Guid>(
                    Error.Forbidden("Review.NotOwner", "Only the client who placed this order can review it."));
            }

            if (order.Status != OrderStatus.Delivered)
            {
                return Result.Failure<Guid>(
                    Error.Validation("Review.OrderNotDelivered", "You can only review orders that have been delivered."));
            }

            var dishInOrder = order.OrderItems.Any(oi => oi.DishId == request.DishId);
            if (!dishInOrder)
            {
                return Result.Failure<Guid>(
                    Error.Validation("Review.DishNotInOrder", "This dish is not part of the specified order."));
            }

            var existingReview = await _unitOfWork.Reviews
                .FindAsync(r => r.OrderId == request.OrderId && r.DishId == request.DishId);

            if (existingReview.Any())
            {
                return Result.Failure<Guid>(
                    Error.Conflict("Review.AlreadyExists", "You have already reviewed this dish for this order."));
            }

            var review = new Review
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                DishId = request.DishId,
                ClientId = request.ClientId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(review.Id);
        }
    }
}