using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.DishImages.Commands.SetPrimaryDishImage
{
    public class SetPrimaryDishImageCommandHandler
        : IRequestHandler<SetPrimaryDishImageCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SetPrimaryDishImageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SetPrimaryDishImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _unitOfWork.dishImages.GetByIdAsync(request.ImageId);

            if (image == null)
            {
                return Result.Failure(
                    Error.NotFound("DishImage.NotFound", "Image not found."));
            }

            var dish = await _unitOfWork.dishes.GetByIdAsync(image.DishId);

            if (dish == null)
            {
                return Result.Failure(
                    Error.NotFound("Dish.NotFound", "Dish not found."));
            }

            if (dish.ChefId != request.ChefId)
            {
                return Result.Failure(
                    Error.Forbidden(
                        "Dish.NotOwner",
                        "Only the owner can update this dish's images."));
            }

            var images = await _unitOfWork.dishImages.GetAllAsync();

            foreach (var img in images.Where(i => i.DishId == image.DishId && i.IsPrimary))
            {
                img.IsPrimary = false;
            }

            image.IsPrimary = true;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}