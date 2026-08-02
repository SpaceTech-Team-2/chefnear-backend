using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.Profile.UploadProfileImage;

public class UploadProfileImageCommandHandler
    : IRequestHandler<UploadProfileImageCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;

    public UploadProfileImageCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
    }

    public async Task<Result> Handle(
        UploadProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);

        if (user == null)
        {
            return Result.Failure(
                Error.NotFound("User.NotFound", "User not found."));
        }

        if (!string.IsNullOrWhiteSpace(user.PhotoUrl))
        {
            try
            {
                await _fileStorageService.DeleteImageAsync(
                    user.PhotoUrl,
                    cancellationToken);
            }
            catch
            {
            }
        }

        await using var stream = new MemoryStream(request.FileBytes);

        var imageUrl = await _fileStorageService.UploadImageAsync(
            stream,
            request.FileName,
            cancellationToken);

        user.PhotoUrl = imageUrl;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}