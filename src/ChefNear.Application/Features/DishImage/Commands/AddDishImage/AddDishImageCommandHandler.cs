using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.DishImages.Commands.AddDishImage
{
    public class AddDishImageCommandHandler
        : IRequestHandler<AddDishImageCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;

        public AddDishImageCommandHandler(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<Guid>> Handle(AddDishImageCommand request, CancellationToken cancellationToken)
        {
            var dish = await _unitOfWork.dishes.GetByIdAsync(request.DishId);

            if (dish == null || dish.IsDeleted)
            {
                return Error.NotFound(
                    "Dish.NotFound",
                    "Dish not found.");
            }

            if (dish.ChefId != request.ChefId)
            {
                return Error.Forbidden(
                    "Dish.NotOwner",
                    "Only the owner can add images to this dish.");
            }

            await using var stream = new MemoryStream(request.FileBytes);

            var imageUrl = await _fileStorageService.UploadImageAsync(
                stream,
                request.FileName,
                cancellationToken);

            var images = await _unitOfWork.dishImages.GetAllAsync();

            if (request.IsPrimary)
            {
                foreach (var img in images.Where(i => i.DishId == request.DishId && i.IsPrimary))
                {
                    img.IsPrimary = false;
                }
            }

            var image = new Domain.Entities.DishImage
            {
                DishId = request.DishId,
                ImageUrl = imageUrl,
                IsPrimary = request.IsPrimary,
                DisplayOrder = images.Count(i => i.DishId == request.DishId)
            };

            await _unitOfWork.dishImages.AddAsync(image);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(image.Id);
        }
    }
}