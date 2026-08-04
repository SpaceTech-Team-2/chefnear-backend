using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Auth.Commands.Profile.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Queries.Profile.GetMyProfile;

public class GetMyProfileQueryHandler
    : IRequestHandler<GetMyProfileQuery, Result<ProfileDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMyProfileQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProfileDto>> Handle(
        GetMyProfileQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);

        if (user == null)
        {
            return Result.Failure<ProfileDto>(
                Error.NotFound("User.NotFound", "User not found."));
        }

        var dto = new ProfileDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            DisplayName = user.DisplayName ?? user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            PhotoUrl = user.PhotoUrl,
            Role = user.Role.ToString()
        };

        return Result.Success(dto);
    }
}