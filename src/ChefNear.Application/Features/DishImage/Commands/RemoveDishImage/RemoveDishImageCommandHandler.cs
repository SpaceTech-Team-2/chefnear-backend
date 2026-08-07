using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.DishImages.Commands.RemoveDishImage
{
    public class RemoveDishImageCommandHandler
        : IRequestHandler<RemoveDishImageCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;

        public RemoveDishImageCommandHandler(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result> Handle(RemoveDishImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _unitOfWork.DishImages.GetByIdAsync(request.ImageId);

            if (image == null)
            {
                return Result.Failure(
                    Error.NotFound("DishImage.NotFound", "Image not found."));
            }

            var dish = await _unitOfWork.Dishes.GetByIdAsync(image.DishId);

            if (dish == null)
            {
                return Result.Failure(
                    Error.NotFound("Dish.NotFound", "Dish not found."));
            }

            if (dish.ChefId != request.ChefId)
            {
                return Result.Failure(
                    Error.Forbidden("Dish.NotOwner", "Only the owner can remove this image."));
            }

            var wasPrimary = image.IsPrimary;
            var imageUrl = image.ImageUrl;

            await _unitOfWork.DishImages.DeleteAsync(image);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            try
            {
                await _fileStorageService.DeleteImageAsync(imageUrl, cancellationToken);
            }
            catch
            {
            }

            if (wasPrimary)
            {
                var images = await _unitOfWork.DishImages.GetAllAsync();

                var nextImage = images
                    .Where(i => i.DishId == dish.Id)
                    .OrderBy(i => i.DisplayOrder)
                    .FirstOrDefault();

                if (nextImage != null)
                {
                    nextImage.IsPrimary = true;
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }

            return Result.Success();
        }
    }
}