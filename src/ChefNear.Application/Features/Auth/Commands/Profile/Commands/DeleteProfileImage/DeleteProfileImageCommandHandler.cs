using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.Profile.Commands.DeleteProfileImage;

public class DeleteProfileImageCommandHandler
    : IRequestHandler<DeleteProfileImageCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;

    public DeleteProfileImageCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
    }

    public async Task<Result> Handle(
        DeleteProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);

        if (user == null)
        {
            return Result.Failure(
                Error.NotFound("User.NotFound", "User not found."));
        }

        if (string.IsNullOrWhiteSpace(user.PhotoUrl))
        {
            return Result.Failure(
                Error.Validation("User.NoProfileImage", "User has no profile image to delete."));
        }

        try
        {
            await _fileStorageService.DeleteImageAsync(user.PhotoUrl, cancellationToken);
        }
        catch
        {
        }

        user.PhotoUrl = null;
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}